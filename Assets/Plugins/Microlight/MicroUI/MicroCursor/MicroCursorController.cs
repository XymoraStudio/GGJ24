using System;
using UnityEngine;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Controller for Micro cursor
    // ****************************************************************************************************
    public class MicroCursorController : MicroUIController {
        CursorSpriteSet _spriteSet;
        CursorStateData _currentStateData;

        internal MicroCursorController(CursorSpriteSet spriteSet, float frameRate = 0.1f) {
            _spriteSet = spriteSet;
            _frameRate = frameRate;

            _locked = false;
            _processing = false;
            CurrentState = MicroCursorStates.Normal;
            LoadSpriteSetData();

            CheckReferences();
        }
        protected void CheckReferences() {
            if(_spriteSet == null) {
                //Debug.LogWarning("MicroCursor: SpriteSet is not referenced.");
            }
            if(_spriteSet != null && _spriteSet.GetCursorStateData(MicroCursorStates.Normal).Textures.Length < 1) {
                //Debug.LogWarning("MicroCursor: Normal cursor set data doesn't have any textures.");
            }
        }

        // Properties
        MicroCursorStates _currentState;
        public MicroCursorStates CurrentState {
            private get => _currentState;
            set {
                if(value == _currentState) return;

                _currentState = value;
                LoadSpriteSetData();
                OnStateChange?.Invoke();   // If not in processing state
            }
        }
        bool _locked = false;
        public bool Locked {
            get => _locked;
            set {
                if(value == _locked) return;

                _locked = value;
                LoadSpriteSetData();
                OnLockedChange?.Invoke();
            }
        }
        MicroCursorStates _lockedState;
        public MicroCursorStates LockedState {
            get => _lockedState;
            set {
                if(value == _lockedState) return;

                _lockedState = value;
                LoadSpriteSetData();
            }
        }
        bool _processing;   // Easy way to set cursor to processing without locking state
        public bool Processing {
            get => _processing;
            set {
                if(_processing == value) return;

                _processing = value;
                LoadSpriteSetData();
                OnProcessingChange?.Invoke();
            }
        }
        bool _dragging;   // Easy way to set cursor to dragging without locking state
        public bool Dragging {
            get => _dragging;
            set {
                if(_dragging == value) return;

                _dragging = value;
                LoadSpriteSetData();
                OnDraggingChange?.Invoke();
            }
        }

        // Variables
        float _frameRate;
        float _frameTimer;
        int _currentFrame;

        // Events
        public static Action OnStateChange;
        public static Action OnLockedChange;
        public static Action OnProcessingChange;
        public static Action OnDraggingChange;

        void LoadSpriteSetData() {
            if(Locked) LoadCursorData(LockedState);
            else if(Processing) LoadCursorData(MicroCursorStates.Waiting);
            else if(Dragging) LoadCursorData(MicroCursorStates.Dragging);
            else LoadCursorData(CurrentState);

            _currentFrame = 0;
            _frameTimer = _frameRate;
            //Cursor.SetCursor(_currentStateData.Textures[_currentFrame], _currentStateData.Offset, CursorMode.Auto);

            // Loads cursor resource into controller
            void LoadCursorData(MicroCursorStates state) {
                if(_spriteSet == null) return;
                _currentStateData = _spriteSet.GetCursorStateData(state);

                if(_currentStateData.Textures.Length < 1) {
                    _currentStateData = _spriteSet.GetCursorStateData(MicroCursorStates.Normal);
                }
            }
        }

        #region MicroUI methods
        override internal void Update() {
            return;
            if(_currentStateData.Textures.Length < 2) return;

            // For animating cursor
            _frameTimer -= Time.deltaTime;
            if(_frameTimer < 0) {
                _frameTimer = _frameRate;
                _currentFrame = (_currentFrame + 1) % _currentStateData.Textures.Length;
                Cursor.SetCursor(_currentStateData.Textures[_currentFrame], _currentStateData.Offset, CursorMode.Auto);
            }
        }
        #endregion
    }
}
using Godot;
using System;

public static class GameResources {
    #region Inputs
        public static string KeyMoveLeft = "move_left";
        public static string KeyMoveRight = "move_right";
        public static string KeyJump = "jump";
    #endregion

    #region Physiscs
        public static float Gravity = 9.8F;
    #endregion

    #region Autoloads
        public static string GlobalAutoload = "/root/Global";
    #endregion

    #region Animations
        public static string idleAnimation = "idle";
		public static string hurtAnimation = "hurt";
		public static string walkAnimation = "walk";
		public static string runAngryAnimation = "run_angry";
    #endregion
}

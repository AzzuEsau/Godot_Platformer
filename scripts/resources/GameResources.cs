using Godot;
using System;
using System.Collections.Generic;

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

    #region Scenes
        private static string levelsPath = "res://scenes/levels/";
        public static Dictionary<int, string> levels = new Dictionary<int, string>() {
            {0, "res://scenes/main_menu.tscn"},
            {1, levelsPath + "level_1.tscn"}
        };
    #endregion

    #region Animations
        public static string idleAnimation = "idle";
		public static string hurtAnimation = "hurt";
		public static string walkAnimation = "walk";
		public static string runAngryAnimation = "run_angry";
		public static string jumpAnimation = "jump";
		public static string doubleJumpAnimation = "double_jump";
		public static string fallAnimation = "fall";
		public static string wallSlideAnimation = "wall_slide";
    #endregion
}

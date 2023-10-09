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
        public static string MainMenuScene = "res://scenes/main_menu.tscn";
        public static string Level1Scene = "res://scenes/main.tscn";

        public static Dictionary<int, string> levels = new Dictionary<int, string>() {
            {0, "res://scenes/main_menu.tscn"},
            {1, "res://scenes/main.tscn"}
        };
    #endregion

    #region Animations
        public static string idleAnimation = "idle";
		public static string hurtAnimation = "hurt";
		public static string walkAnimation = "walk";
		public static string runAngryAnimation = "run_angry";
    #endregion
}

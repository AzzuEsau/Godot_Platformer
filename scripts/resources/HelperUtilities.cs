using Godot;
using System;

public static class HelperUtilities {
    #region Validations
        public static bool ValidateCheckNullValue(Node thisObject, string fieldName, Node objectToCheck) {
            if(objectToCheck == null)
                GD.PushError(fieldName + " is null and must contain value in object " + thisObject.Name);
            return objectToCheck == null;
        }


        public static bool ValidateCheckPositiveValue(Node thisObject, string fieldName, int valueToCheck, bool isZeroAllowed) {
            if(isZeroAllowed && valueToCheck < 0) {
                GD.PushError(fieldName + " must contain a positive value or zero in object " + thisObject.Name);
                return true;
            }
            if(!isZeroAllowed && valueToCheck <= 0) {
                GD.PushError(fieldName + " must contain a positive value in object " + thisObject.Name);
                return true;
            }
            return false;
        }

        public static bool ValidateCheckPositiveValue(Node thisObject, string fieldName, float valueToCheck, bool isZeroAllowed) {
            if(isZeroAllowed && valueToCheck < 0) {
                GD.PushError(fieldName + " must contain a positive value or zero in object " + thisObject.Name);
                return true;
            }
            if(!isZeroAllowed && valueToCheck <= 0) {
                GD.PushError(fieldName + " must contain a positive value in object " + thisObject.Name);
                return true;
            }
            return false;
        }
    #endregion

}
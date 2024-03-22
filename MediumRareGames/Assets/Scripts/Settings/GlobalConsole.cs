/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   GlobalConsole
       - Easy way to log messages to the Unity Console, tagged with the script (and optionally the object)

   Details:
       - Include using ConsoleLogging
       - Call: Log.Message(this, string); for a tagged log
       - Call: Log.ObjectMessage(this, string); for a tagged log + the object
       - Same thing for warnings and errors
       - You can also use the overload for setting a base color for messages, warnings and errors
-----------------------------------------------------------------------------
*/

using UnityEngine;

namespace ConsoleLogging
{
    public class Log
    {
        #region Messages
        /// <summary>
        /// Print a normal message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void Message(MonoBehaviour sender, string text)
        {
            Debug.Log("<b>" + sender.GetType().ToString() + "</b>: " + text);
        }

        /// <summary>
        /// Print a normal message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void Message(MonoBehaviour sender, string text, Color color)
        {
            Debug.Log("<b>" + sender.GetType().ToString() + "</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }

        /// <summary>
        /// Print a normal message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void ObjectMessage(MonoBehaviour sender, string text)
        {
            Debug.Log("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: " + text);
        }

        /// <summary>
        /// Print a normal message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void ObjectMessage(MonoBehaviour sender, string text, Color color)
        {
            Debug.Log("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }
        #endregion

        #region Warnings
        /// <summary>
        /// Print a warning message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void Warning(MonoBehaviour sender, string text)
        {
            Debug.LogWarning("<b>" + sender.GetType().ToString() + "</b>: " + text);
        }

        /// <summary>
        /// Print a warning message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void Warning(MonoBehaviour sender, string text, Color color)
        {
            Debug.LogWarning("<b>" + sender.GetType().ToString() + "</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }

        /// <summary>
        /// Print a warning message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void ObjectWarning(MonoBehaviour sender, string text)
        {
            Debug.LogWarning("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: " + text);
        }

        /// <summary>
        /// Print a warning message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void ObjectWarning(MonoBehaviour sender, string text, Color color)
        {
            Debug.LogWarning("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }
        #endregion

        #region Errors
        /// <summary>
        /// Print a error message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void Error(MonoBehaviour sender, string text)
        {
            Debug.LogError("<b>" + sender.GetType().ToString() + "</b>: " + text);
        }

        /// <summary>
        /// Print a error message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void Error(MonoBehaviour sender, string text, Color color)
        {
            Debug.LogError("<b>" + sender.GetType().ToString() + "</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }

        /// <summary>
        /// Print a error message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        public static void ObjectError(MonoBehaviour sender, string text)
        {
            Debug.LogError("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: " + text);
        }

        /// <summary>
        /// Print a error message to the console, tagged with the sender (script calling).
        /// </summary>
        /// <param name="sender">The script calling this function (use "this")</param>
        /// <param name="text">Message to print.</param>
        /// <param name="color">Base color to set message to (using HTMl-specified colors in the text can override this)</param>
        public static void ObjectError(MonoBehaviour sender, string text, Color color)
        {
            Debug.LogError("<b>" + sender.GetType().ToString() + " (on \"" + sender.name + "\")</b>: <color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>");
        }
        #endregion
    }
}

Custom Handles readme
================================================================================
Although alpha and beta releases of Unity are not officially supported, this
package will sometimes be updated during test cycles to add support as feasible.

PATCH RELEASES ARE NOT OFFICIALLY SUPPORTED. USE THEM AT YOUR OWN RISK.

More and up to date information available at
http://developers.candlelightinteractive.com/


Installing Custom Handles
--------------------------------------------------------------------------------

Simply import the package and everything should "just work." Please see the
examples in the package for ideas on how you could use these tools. If you wish
to publish packages that depend on this one, you can do so with the
IS_CANDLELIGHT_CUSTOM_HANDLES_AVAILABLE symbol.

If you experience any problems, please open the Unity preferences menu, navigate
to the Candlelight section on the left, select the Custom Handles tab, and use
the buttons at the bottom to report a bug or visit the support forum.


Using the Package
--------------------------------------------------------------------------------

All code is in the Candlelight namespace. The general usage pattern is to create
a custom editor script for your desired component, and implement OnSceneGUI().
In this method, wrap any calls to custom handle methods using
Candlelight.SceneGUI.BeginHandles() and Candlelight.SceneGUI.EndHandles(). When
the latter method returns true, a change was detected and you can apply new
values to your target object. All custom handle classes are in the folder
Plugins/Editor/Candlelight/Library/Handles. You can see example custom editors
in the folder Plugins/Editor/Candlelight/Library/Editors.


Known Issues
--------------------------------------------------------------------------------

None
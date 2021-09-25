# Settings-System

 Advanced Settings System for Unity.
 
 It's not ready yet, but already usable I believe. It supports automatic saving to PlayerPrefs, all basic types like float, int, bool and even special type for volume sliders that automaticly detect all the exposed parameters in the selected mixer. It also supports key bindings, however it uses the old input system. 
 You can derive from Setting<T> class to create your own setting type. It will be automaticly found by the system using System.Reflection.

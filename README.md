# Settings-System

 Advanced Settings System for Unity.
 
 It's not ready yet, but already useable i believe. It supports automatic saving to PlayerPrefs, supports all base types like float, int, bool and even special type for volume sliders that automaticly detects all the exposed parameters in the selected mixer. Also supports Key Bindings, however it uses the old input system. 
  You can derive from Setting<T> class to create your own setting types. They will be found by the system using C# Reflection.

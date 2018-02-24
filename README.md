# injectLoader
Runtime assembly reload for Unity
Allows assemblies to be loaded repeatedly in the same session.

### Usage
1. Install IPA into the target.
2. Build injectLoader and install into `Plugins`
3. Launch target and press `LeftAlt`, this generates `UserData/modprefs.ini`
4. Edit `path` under the `[injectLoader]` key to point at your plugin DLL.
5. Implement a bootstrap method in your plugin dll.
6. Build your plugin.
7. Use `LeftAlt` to trigger reload.

Repeat steps 6 & 7 ad nauseam.

### Bootstrap method

injectLoader attempts to invoke a specific method in the plugin DLL upon reload.
This method requires a hardcoded namespace, class name and signature.
Given that the aim of plugin development is often to execute level-specific logic, the Bootstrap method can instantiate and execute existing plugin code.

```csharp
namespace script
{
    public class Main
    {
        public static void Bootstrap()  //Hardcoded bootstrap method.
        {
            //Perform actions here.
            new SomeNamespace.ExamplePlugin().OnLevelWasLoaded(-1);   //Reload plugin code.
        }
    }
}

namespace SomeNamespace
{
    public class ExamplePlugin : IPlugin
    {
        public string Name => nameof(ExamplePlugin);
        public string Version => "1.0";

        public void OnApplicationQuit() { }
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnUpdate() { }
        public void OnApplicationStart() { }

        public void OnLevelWasLoaded(int level)
        {
            //...
        }
    }
}
```

### Dependencies
* IPA
* dnlib
* MSBuild.ILMerge.Task
# ACT-R-Client (**under development**)
This library makes an ACT-R Client available for .NET (>= 3.5) projects.
It needs [ACT-R Version 7.6](http://act-r.psy.cmu.edu/act-r-7-6/) (currently in beta) or higher since this is the first version with the new JSON-RPC interface called dispatcher.
I highly recommend to read the *ACT-R RPC Interface Documentation* for a better understandig how the ACT-R dispatcher works.

## How to use the ACT-R Client
You can install the library to your project via Nuget:
```
Install-Package ACT-R-Client
```
Afterwards you can create an *ActRClient* object:
```csharp
// since ActRClient implements IDisposal it can be used in an using block
using (ActRClient actr = new ActRClient("127.0.0.1", 2650)) // change host and port to your needs
{
  // do what you need to do
}
```
With this object you can interact with the ACT-R dispatcher.

### Add / Remove Command to the dispatcher
For adding commands to the dispatcher you have to create an *AbstractAddCommandRequest* instance and send it to the dispatcher:
```csharp
// add command
AbstractAddCommandRequest addCommandRequest = new AddCommandRequest(KeyPressAction, "published-name",
  "private-name", "Documentation");
actr.AddDispatcherCommand(addCommandRequest);

// remove command
actr.RemoveDispatcherCommand(addCommandRequest);
// or
actr.RemoveDispatcherCommand("private-name");
```
The *AddCommandRequest* is a implementation of the abstract class *AbstractAddCommandRequest*, which uses a simple delegate for command evaluations. Of course you can implement your own *AbstractAddCommandRequest* class to handle the evaluation requests from the dispatcher as you want to.

### Add / Remove Monitor to the dispatcher
Adding a command monitor is very similar to adding a command:
```csharp
// add monitor
MonitorRequest dispatcherMonitor = new MonitorRequest("command-to-monitor", "command-to-call");
actr.AddDispatcherMonitor(dispatcherMonitor);

//remove monitor
actr.RemoveDispatcherMonitor(dispatcherMonitor);
// or
actr.RemoveDispatcherMonitor("command-to-monitor", "command-to-call");
```

### Evaluate a command by the dispatcher
There are about 100 implemented "ready to use" methods for the most common ACT-R functions, like *load-act-r-model*, *reset*, *run*, *...*
```csharp
// load model
actr.LoadActrModel("ACT-R:tutorial;unit2;demo2-model.lisp");
// reset ACT-R scheduler and model
actr.Reset();
// open experiment window
Window window = actr.OpenExpWindow("Letter difference", true);
// add text to experiment window
actr.AddTextToWindow(window, "A", 125, 150);
// install device
actr.InstallDevice(window);
// run model
actr.Run(10, true);
```
At the moment only a few are tested, so erros can be occure (please let me know if you found one and i will fix it).
If you want to make a different request you have to implement the abstract class *AbstractEvaluationRequest*, adding all necessary parameter to the *BaseParameterList* within the *ToParameterArray* method. This *AbstractEvaluationRequest* can then be send to the dispatcher.
```csharp
// example AbstractEvaluationRequest implementation for the "my-command"
public class MyCommand : AbstractEvaluationRequest
{
  public MyCommand(int intPara, bool boolPara, string stringPara, string model = null) : base("my-command", model)
  {
    IntPara = intPara;
    BoolPara = boolPara;
    StringPara = stringPara;
  }
  public int IntPara { set; get; }
  public bool BoolPara { set; get; }
  public string StringPara { set; get; }

  public override object[] ToParameterArray()
  {
    var list = BaseParameterList();

    list.Add(IntPara);
    list.Add(BoolPara);
    list.Add(StringPara);

    return list.ToArray();
    }
}

// let the dispatcher evaluate my-command
AbstractEvaluationRequest myCommandRequest = new MyCommand(0, false, "StringParameter");
Result result = SendEvaluationRequest(myCommandRequest);
```

## Example
The [Example Project](https://github.com/nyctico/ACT-R-Client/tree/master/Example) contains some implementations of the ACT-R tutorials.

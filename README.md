# NotificationCenter  [![Build Status](https://travis-ci.org/Husseinhj/NotificationCenter.svg?branch=master)](https://travis-ci.org/Husseinhj/NotificationCenter) ![Nuget version](https://img.shields.io/nuget/v/NotificationCenter.svg?style=flat) ![downloads](https://img.shields.io/nuget/dt/NotificationCenter.svg?style=flat)
This library works like Objective-C and Swift NSNotificationCenter.
A notification dispatch mechanism that enables the broadcast of information to registered observers.

## Installation 
Use this command in Nuget Package Manager Console:

```
PM> Install-Package NotificationCenter
```

## Methods
### Subscribe
To add an action with Key in NotificationCenter, You should use this code :

``` csharp
NotificationCenter.Subscribe("KEY",Action);

private void Action()
{
    Debug.WriteLine("Action was run");
}

// or
NotificationCenter.Subscribe("KEY",Action);

private void Action(object o)
{
    Debug.WriteLine("Action was run with {0} object",o);
}
```

### Unsubscribe
To remove your action in NotificationCenter, You should use this code :

``` csharp
NotificationCenter.Unsubscribe("KEY");
```

### Notify
To invoke or notify all actions in unique Key, You should use this code :

``` csharp
NotificationCenter.Notify(key: "KEY",data: 5);
```

### KeepActionValue property
Keep action data if key was not subscribed yet. It just work in Notify with data.

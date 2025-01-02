## HOMEPAGE

GameFrameX 的 Timer 计时器组件

**Timer 计时器组件 (Timer Request)** - 提供计时器相关的接口。

# 使用文档(文档编写于GPT4)

# TimerComponent 功能说明文档

## 概述

`TimerComponent` 是一个Unity组件，用于在Unity项目中管理和处理计时任务。它允许开发者添加各种定时执行的任务，比如定时触发事件、重复执行或者每帧更新执行的任务。

## 方法说明

### Add

添加一个定时调用的任务。

**参数：**

- `float interval`：间隔时间，以毫秒为单位。
- `int repeat`：重复次数，0 表示任务会无限重复。
- `Action<object> callback`：任务触发时执行的回调函数。
- `object callbackParam`（可选）：回调函数的参数。

**用法：**

```csharp
Add(1000, 5, MyMethod);
```

### AddOnce

添加一个只执行一次的定时任务。

**参数：**

- `float interval`：间隔时间，以毫秒为单位。
- `Action<object> callback`：任务触发时执行的回调函数。
- `object callbackParam`（可选）：回调函数的参数。

**用法：**

```csharp
AddOnce(5000, MyMethod);
```

### AddUpdate

添加一个每帧更新执行的任务。

**参数：**

- `Action<object> callback`：每帧执行的回调函数。
- `object callbackParam`（可选）：回调函数的参数。

**用法：**

```csharp
AddUpdate(MyMethod);
```

### Exists

检查指定的任务是否存在。

**参数：**

- `Action<object> callback`：要检查的回调函数。

**返回：**

- `bool`：存在返回 `true`，不存在返回 `false`。

**用法：**

```csharp
bool doesExist = Exists(MyMethod);
```

### Remove

移除指定的任务。

**参数：**

- `Action<object> callback`：要移除的回调函数。

**用法：**

```csharp
Remove(MyMethod);
```

## 使用案例

要使用`TimerComponent`，需要先在您的Unity项目中将此组件添加到游戏对象上。以下是如何使用这个组件的一个简单示例：

```csharp
// 假设这是您要触发的方法
void MyMethod(object param)
{
    Debug.Log("方法被触发");
}

// 创建定时任务，每隔1秒执行MyMethod方法，共执行5次
GetComponent<TimerComponent>().Add(1000, 5, MyMethod);

// 创建定时任务，5秒后执行一次MyMethod方法
GetComponent<TimerComponent>().AddOnce(5000, MyMethod);

// 检查是否存在某个任务
bool exists = GetComponent<TimerComponent>().Exists(MyMethod);

// 移除特定的任务
GetComponent<TimerComponent>().Remove(MyMethod);
```

## 注意事项

- 在使用定时器前，请确保`TimerManager`已正确初始化并且已正确注入到`GameFrameworkEntry`模块中。
- 尽量避免使用过小的间隔时间（例如小于帧时间），这可能会导致性能问题。

# 使用方式(任选其一)

1. 直接在 `manifest.json` 的文件中的 `dependencies` 节点下添加以下内容
   ```json
      {"com.gameframex.unity.timer": "https://github.com/AlianBlank/com.gameframex.unity.timer.git"}
    ```
2. 在Unity 的`Packages Manager` 中使用`Git URL` 的方式添加库,地址为：https://github.com/AlianBlank/com.gameframex.unity.timer.git

3. 直接下载仓库放置到Unity 项目的`Packages` 目录下。会自动加载识别
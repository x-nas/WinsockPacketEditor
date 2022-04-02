# WinPacketsEdit

封包拦截器，可以自适应拦截32位和64位的手游封包

用C#自己开发了一个仿WPE的封包拦截器，可以自适应32位和64位的软件，且wpe常用的功能都有了，开发中使用了多线程技术，测试了拦截1万+的封包不会卡死或退出，软件不定期会更新功能，每次启动的时候支持在线自动更新，欢迎大家一起完善和改进！

Winsocket packet interceptor, which can intercept mobile game  with simulator, supports 32-bit and 64 bit App 网络封包拦截器，可拦截手游封包，支持32位及64位程序

Download setup.exe: https://www.x-nas.com/wpe/publish.htm

![31](https://user-images.githubusercontent.com/67667226/161363934-8ba0333c-c72c-498b-8c15-86845e03f72b.jpg)

![32](https://user-images.githubusercontent.com/67667226/161363944-e77c0b47-ac6f-4bb3-8d44-4bea6bf849c0.jpg)

安装版本使用方法：  
  1. 下载解压缩后双击运行安装程序图标 
  
  ![1](https://user-images.githubusercontent.com/67667226/161364663-ce7a01aa-f359-458c-ae9b-b7468803b19d.jpg)

  
  2. 如有提示请选择 “更多信息”，并点击 “仍要运行” 
  
  ![2](https://user-images.githubusercontent.com/67667226/161364780-101447f1-ed3e-4c76-9980-43605a45711a.jpg)

  
  3. 安装程序会自动检测操作系统的运行环境，如有需要会提示安装相关运行库（比如.net 4.0等），点击下载及安装即可.
  
  ![3](https://user-images.githubusercontent.com/67667226/161364787-0d028d2d-95b7-4771-a0f6-650542709f44.jpg)

  
  4. 安装完成后启动时如有提示，请选择 “更多信息”，并点击 “仍要运行”
    
  ![4](https://user-images.githubusercontent.com/67667226/161364794-11c37f22-39f6-4c52-86cc-0acd41a7362f.jpg)
 
启动并注入进程：
  1. 双击软件图标启动程序后，会提示以管理员权限运行，点击确定后显示进程注入界面，点击 “...” 按钮选择进程
  
  ![5](https://user-images.githubusercontent.com/67667226/161364836-3c089719-ae78-4089-ac2f-48d797600cde.jpg)

  
  2. 选择需要拦截封包的进程名称，点击“确定”，如下面图示选择的为雷电模拟的进程
  
  ![6](https://user-images.githubusercontent.com/67667226/161364842-a1c97049-296a-4bfc-ac29-9cd03679d427.jpg)

  
  3. 选择进程后，点击 “注入” 按钮
  
  ![7](https://user-images.githubusercontent.com/67667226/161364853-2c8760bd-4632-4dac-b76d-74f41a81876e.jpg)

  
  4. 如下图出现红框内文字时代表注入进程成功，否则会提示相关错误信息（并不是所有进程都可以实现注入，各类手游模拟器及CCProxy等代{过}{滤}理软件经测试可以成功注入）
  
  ![8](https://user-images.githubusercontent.com/67667226/161364857-e3a00761-866d-41f5-9cde-b2a621f335d6.jpg)

  
  5. 成功注入进程后，即可弹出封包拦截主界面，这时候即可关闭上图的进程注入器了
 
  封包拦截器的使用：  
  封包拦截器是仿WPE的常用功能很容易上手我就不做详细的说明了，如果后期有需要的话我再补充这块的说明。
  
  软件的更新及卸载：
  1. 软件在每次启动的时候会自动更新，我会不定期的修改bug和完善功能。
  
  
  
  2. 卸载软件 “控制面板” - “卸载软件” - “封包拦截器”

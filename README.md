<div align="center">
<p><img src="https://www.wpe64.com/web_images/wpe.png" height="150"></p>

# Winsock Packet Editor（WPE x64）

<img src="https://img.shields.io/github/license/x-nas/WinsockPacketEditor" alt="License"></img>
[![Visitors](https://visitor-badge.laobi.icu/badge?page_id=x-nas.WinsockPacketEditor&title=Visitors)](https://github.com/x-nas/WinsockPacketEditor)
![GitHub Repo stars](https://img.shields.io/github/stars/x-nas/WinsockPacketEditor?style=dark)
![GitHub Repo forks](https://img.shields.io/github/forks/x-nas/WinsockPacketEditor?style=dark)
[![star](https://gitee.com/x-nas/WinsockPacketEditor/badge/star.svg?theme=dark)](https://gitee.com/x-nas/WinsockPacketEditor/stargazers)
[![fork](https://gitee.com/x-nas/WinsockPacketEditor/badge/fork.svg?theme=dark)](https://gitee.com/x-nas/WinsockPacketEditor/members)

&bull; <a href="https://www.wpe64.com">官方网站</a>
&bull; <a href="https://www.wpe64.com">Official website</a>

</div>

## 软件简介

WPE x64 是一款可以拦截并修改 WinSock 封包的 Windows 软件，自适应支持 32 位及 64 位的目标程序，软件支持 SOCKS 代理和进程注入两种模式，并且具有高级滤镜和自动化机器人等功能，开发中使用了 C# 的多线程和消息队列技术，测试拦截了 100 万+的封包不会卡死或退出，软件不定期会修复 Bug 和更新功能，每次启动的时候支持在线自动更新.

WPE x64 支持直接注入 Windows 进程来拦截 Winsock 封包，也可以通过 SOCKS 代理模式来拦截 Winsock 封包.

本软件使用了微软的 VS2022 集成开发环境，.NET Framework 4.8 开发框架，以及 ClickOnce 部署资源。每次版本更新后，都会在启动程序时自动下载最新版本。如果更新服务器不可用，也不会导致程序无法使用。当然，如果您不希望自动更新，也可以在启动时手动关闭自动更新，或者直接下载离线打包版使用。

## Introduction

WPE x64 is a Windows software that can intercept and modify WinSock packets, with adaptive support for 32-bit and 64-bit target programs. The software supports two modes: SOCKS proxy and process injection, and has advanced filters and automated robots. It uses C# multi threading and message queue technology in development, and has intercepted over 1 million packets without freezing or exiting. The software fixes bugs and updates periodically, and supports online automatic updates every time it starts.

WPE x64 supports direct injection into Windows processes to intercept Winsock packets, and can also intercept Winsock packets through SOCKS proxy mode.

This software uses Microsoft's VS2022 integrated development environment NET Framework 4.8 development framework, as well as GTK deployment resources. After each version update, the latest version will be automatically downloaded when the program is launched. If the update server is unavailable, it will not cause the program to be unusable. Of course, if you do not want automatic updates, you can manually turn off automatic updates at startup or download the offline packaged version directly for use.

## 软件特色

- [x] 支持 SOCKS 代理和进程注入两种模式，确保在各种情况下都可以拦截到 Winsock 封包.
- [x] 代理模式下支持多种主流代理协议和 SSL 安全协议，并具有端口映射和断点调试等功能.
- [x] 具备自动化的可编程机器人功能，可在满足触发条件的情况下执行预定义的指令集.
- [x] 消息队列缓存模式，所有的封包依次排队进入 MQ 队列，无需等待缓存结束后再显示封包.
- [x] 您可以自定义需要拦截的封包类型，已包含 WinSock 1.1 和 2.0 的 APIs.
- [x] 注入器和封包编辑器相对独立，可一次注入多个软件后，分别获取不同程序的网络封包.
- [x] 您可以通过选择一个尚未运行的程序注入后，从启动阶段即开始获取程序的所有封包数据.
- [x] 直观的封包对比功能，支持多种数据格式之间快速切换.
- [x] 您可以方便的对封包内容进行搜索，支持多种数据格式的快速搜索定位.
- [x] 支持批量发送封包，您可以自定义发送的顺序和循环次数，并支持导入导出和备注功能.
- [x] 强大的滤镜功能，支持高级滤镜，并且可以自定义修改封包的长度和修改次数.
- [x] 支持注入 Winsock 代理程序后，再获取目标程序的网络封包.
- [x] 您可以直接注入各类模拟器，并直接获取模拟器以及运行的程序的网络封包.
- [x] 您对系统的各种配置都会及时的进行保存，下次启动软件时会自动带出上一次的设置.
- [x] 软件运行期间会实时记录运行日志并支持导出，方便定位问题和提交处理.
- [x] 支持 64 位的 Windows 操作系统和 64 位的目标程序，并且会根据目标进程的类型来自动调用 32 位或 64 位的动态库注入目标程序.
- [x] 软件使用的 .NET 程序集不需要在全局程序集缓存（GAC）中注册，大大简化了使用和二次开发.
- [x] 支持多线程技术，处理封包时不会影响程序的正常操作.
- [x] 拦截封包结束后会自动处理挂钩并释放资源，避免对程序运行产生影响.
- [x] 不会使目标程序产生资源和内存泄露风险.
- [x] 软件安装时会自动检测必须的组件和运行库，确保NET框架已安装.
- [x] 采用微软 ClickOnce 发布技术，支持在线自动安装和更新.
- [x] 支持多语言版本，方便不同国家和地区的用户使用.

## Features

- [x] Supports both SOCKS proxy and process injection modes to ensure that Winsock packets can be intercepted in various situations.
- [x] In proxy mode, it supports multiple mainstream proxy protocols and SSL security protocols, and has functions such as port mapping and breakpoint debugging.
- [x] Equipped with automated programmable robot functionality, capable of executing pre-defined instruction sets under triggering conditions.
- [x] Message queue caching mode, where all packets are queued in sequence to enter the MQ queue and processed according to the first in, first out rule, without waiting for the cache to finish before displaying the packets.
- [x] You can customize the packet types that need to be intercepted, which already include APIs for WinSock 1.1 and 2.0.
- [x] The injector and packet editor are relatively independent, and can inject multiple software at once to obtain network packets from different programs separately.
- [x] You can inject a program that has not yet been run and start obtaining all packet data of the program from the startup phase.
- [x] Intuitive packet comparison function, supporting quick switching between multiple data formats.
- [x] You can easily search the contents of the package and support fast search and location of multiple data formats.
- [x] Support batch sending of packets, you can customize the order and number of cycles of sending, and support import/export and remark functions.
- [x] Powerful filter function, supports advanced filters, and can customize the length and modification times of the package.
- [x] Support injecting Winsock proxy program and then obtaining network packets of the target program.
- [x] You can directly inject various simulators and obtain network packets of simulators and running programs directly.
- [x] You will save various configurations of the system in a timely manner, and the next time you start the software, the previous settings will be automatically brought up.
- [x] During software operation, real-time running logs will be recorded and exported, making it easy to locate problems and submit for processing.
- [x] Supports 64 bit Windows operating system and 64 bit target programs, and automatically calls 32-bit or 64 bit dynamic libraries to inject target programs based on the type of target process.
- [x] The NET assembly used by the software does not need to be registered in the Global Assembly Cache (GAC), greatly simplifying usage and secondary development.
- [x] Support multi-threaded technology, processing packets will not affect the normal operation of the program.
- [x] After intercepting packets, hooks will be automatically processed and resources will be released to avoid any impact on program operation.
- [x] Will not pose a risk of resource and memory leakage to the target program.
- [x] During software installation, necessary components and runtime libraries will be automatically detected to ensure that the NET framework is installed.
- [x] Adopting Microsoft ClickOnce release technology, supporting online automatic installation and updates.
- [x] Supports multiple language versions, making it convenient for users from different countries and regions to use.

## 软件界面 Software UI

![Proxy](https://github.com/user-attachments/assets/ba1bfe80-3c1c-4839-aa68-24aa5ddb4738)

![Process](https://github.com/user-attachments/assets/6bfe3e16-cfc0-42c3-987c-26724363adb2)

![111](https://github.com/user-attachments/assets/cb9c6c4d-e742-4789-beb0-94288b105194)
![222](https://github.com/user-attachments/assets/54b81cbb-73b8-43e1-ac49-2368be7a3eb8)




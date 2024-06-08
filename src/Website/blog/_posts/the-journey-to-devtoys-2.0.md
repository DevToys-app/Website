---
title: How DevToys became cross-platform and extensible
description: The long journey of DevToys from a Windows-only tool to a cross-platform one.
category: Developer story
date: 6/13/2024
gitHubAuthorName: veler
---

Just a few days ago, we proudly launched DevToys 2.0, now available on Windows, macOS, and Linux. This significant milestone marks a new chapter in our journey, and we're excited to share the story behind this transformation.

## Some background

When we started working on DevToys, our main goal was to create a set of tools that would make developers' lives easier. We wanted to provide a collection of utilities that would help developers be more productive and efficient in their day-to-day work. Our initial focus was on Windows, as we, [Etienne Baudoux](https://www.linkedin.com/in/etiennebaudoux/) and [Benjamin Titeux](https://www.linkedin.com/in/benjamin-titeux/), had extensive experience with this operating system and had no idea how much DevToys would grow after initial release, and thus, we initially refrained from targeting multiple platforms as it was simpler for us.

As a result, DevToys began as a [UWP](https://learn.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide) app, which is limited to Windows. We received a lot of positive feedback from the community, and the number of downloads kept increasing over time. However, we soon started receiving requests for macOS and Linux support. Initially hesitant, we couldn't ignore the demand as it became [the most upvoted issue on GitHub](https://github.com/DevToys-app/DevToys/issues/156).

Despite our years of experience in software engineering, we had never ventured into cross-platform app development. We knew it would require learning new things, and we were ready to embrace the challenge.

## The challenges ahead of us

In **January 2023**, we embarked on the ambitious project of rewriting DevToys from scratch, this time with cross-platform support in mind. This was a monumental task that pushed us out of our comfort zone and led us to explore various options.

To make this project a reality, we had to overcome several challenges.

### Choosing the Right Programming Language

Our first challenge was selecting the appropriate programming language. We were well-versed in `C#` and the `Microsoft .NET` ecosystem, and we wanted to continue using them to avoid a steep learning curve. We knew that .NET was cross-platform, and we could use it to build applications that run on `Windows`, `macOS`, and `Linux`. We also knew that .NET 8, at that time, was on the horizon, and it would bring even more improvements for cross-platform development.

One of the advantages of .NET is its ability to run natively on the target platform, providing access to all system features and great performances. This isn't always the case with other cross-platform technologies. This advantage would enable us to develop a rich desktop application that could fully leverage the features of the underlying operating system.

However, we were also aware that creating a cross-platform desktop app in .NET wasn't the most popular approach, and that the other challenges we faced would make it even more difficult.

### Extensibility

One of the major changes we wanted to implement in DevToys 2.0 was to make it extensible. We aimed to make the process more efficient for our community to add new tools into DevToys more freely. However, creating an extensible .NET desktop app is no easy task, as .NET isn't as flexible as web technologies when it comes to dynamically loading and running code. This posed significant challenges in terms of architecture and performance, which we will discuss later in this article.

### Text Editor

Have you noticed that in DevToys 1.0, the text editor on multiple lines is rich? For instance, `JSON`, `XML` and `YAML` are colorized, line numbers are displayed, whitespace can be made visible, and a palette command is available when pressing `F1`. This text editor is powered by [Microsoft Monaco Editor](https://microsoft.github.io/monaco-editor/), which is an open-source web-based code editor that powers [Visual Studio Code](https://code.visualstudio.com/) and more. We chose this editor because it offers a rich editing experience and supports many languages, requiring minimal maintenance and development time on our part. We also chose it because we know that many people love Visual Studio Code and would feel at home with this editor.

For DevToys 2.0, unless we find a convincing alternative, we will stick to it. For now, we are confident Monaco Editor is a great option for Windows, macOS and Linux.

### User Interface

[In the reviews of DevToys on the Microsoft Store](https://apps.microsoft.com/detail/9pgcv4v3bk4w?amp%3Bgl=US&hl=en-us&gl=US), people often praise the user interface. They find it beautiful, modern, and integrates well to [Windows 11's design language](https://learn.microsoft.com/en-us/windows/apps/design/), Fluent UI.
In DevToys 2.0, we wanted to maintain this UI that users had come to love. As a bonus, making a beautiful UI on macOS and Linux that "feels like home" would be fantastic!

We knew that achieving this in a cross-platform way would be challenging. So many cross-platform apps out there have a UI that does not respect the design language of any of their hosting system, and instead look the same on every platform. There are often some good reasons for that, such as budget and time constraints, along with not confusing the user with a different UI from one platform to another, and finally, maybe the fact that a lot of users simply do not prioritize the look over the features.

The best way to guarantee a native look and feel app on each platform is simply to make it... native. Developing a fully native app for each platform was not a consideration for us though. As we are developing this app in our free time, we didn't want to have to maintain 3 different codebases, as the amount of shared code would be limited, and it would require much more work for us. It would also make the extensibility aspect difficult, as extensions would likely need to be developed three times, one per platform. Fortunately, there are multiple cross-platform UI frameworks for .NET. The challenge was to find the one that best suited our needs.

## Six Months of Trial and Error

We spent the first six months of 2023 exploring different technologies and frameworks to build DevToys 2.0. As we wished to develop a native-looking app, we started by looking at XAML-based frameworks for .NET.

### Uno Platform

Our first attempt was with [Uno Platform](https://platform.uno/). It had the advantage of using [WinUI 3](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/) on Windows, while reproducing the same look and feel on macOS and Linux. WinUI 3 provides native controls on Windows, using Fluent UI, which aligned with our objective of maintaining a native-looking app on Windows.

At the time we evaluated Uno Platform as an option, on macOS, by default, Uno Platform apps had run as a [Mac Catalyst](https://developer.apple.com/documentation/uikit/mac_catalyst) application, in line with what .NET offers. It emulates the Fluent UI while employing [UIKit](https://developer.apple.com/documentation/uikit) components underneath. Unless we customized the default components in XAML, the app would mirror the Windows version but operate within the Apple ecosystem. Thanks to Mac Catalyst, DevToys would run on Mac and iPad. This was a nice bonus for us, but not something that we particularly needed as the main audience of DevToys is desktop environment users. Since our evaluation, Uno Platform has added support for macOS via Skia in their [5.2 release (May 2024)](https://platform.uno/blog/the-first-and-only-true-single-project-for-mobile-web-desktop-and-embedded-in-net/).

On Linux, at the time of our evaluation, Uno Platform rendered the UI using [Skia](https://skia.org/) and [GTK](https://www.gtk.org/), once again emulating Fluent UI by default. In this case again, Uno Platform 5.2 has updated their default options since our initial evaluation, removing GTK dependency and using `X11/Skia` instead. We did not evaluate this new option as we had already moved on to another solution that we describe later in this article.

Our exploration of Uno Platform in early 2023 quickly led us to several obstacles:
1. Mac Catalyst apps on macOS have a significant issue with keyboard input and the `WKWebView` (the Safari web view, which we use for Monaco Editor). In short, keyboard input is not recognized, posing a major issue when using a web view to power a text editor. The issue does not reproduce on iPad, but this platform is not our target. This issue isn't limited to Uno Platform, [it also impact .NET MAUI](https://github.com/unoplatform/uno/issues/9877#issuecomment-1481593920) and native Mac Catalyst apps running on macOS. At the time of writing this article, it appears that [Apple hasn't yet resolved the problem](https://developer.apple.com/forums/thread/721141).
1. As an alternative, we considered whether Uno Platform could function for a classic, macOS desktop app using [AppKit](https://developer.apple.com/documentation/appkit). However, according to the Uno Platform team, which we thanks for their support, the current state of AppKit support is **_"broken"_**. We don't blame the Uno Platform team here: they focus on iOS/iPad instead of macOS because that's what most of their customers need.
1. As a final alternative, we attempted to find a XAML-based control that provides a rich text-editing experience to replace Monaco Editor, but there was nothing satisfactory at the time, and coding it ourselves would have been too time-consuming.

### .NET MAUI

We also experimented with [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui). Similar to the situation with Uno Platform, on macOS, the app has to be a Mac Catalyst one, so we encountered the same problem with the keyboard input in the web view. Additionally, at the time of writing this article, .NET MAUI does not support Linux at all. The main target audience for the MAUI team is probably smartphone and tablet apps more than desktop apps, which is likely why Linux support isn't a priority.

### Avalonia

[Avalonia](https://avaloniaui.net/) is another cross-platform XAML framework for .NET. Unlike Uno Platform and .NET MAUI, Avalonia, until very recently, specialized in desktop app development. It offers robust support for Windows, macOS, and Linux.

Unfortunately for us, Avalonia does not provide a native look and feel on Windows, macOS and Linux. It has its own styling. We could have customized the style to make it resemble Fluent UI on Windows, but it would have required a significant amount of work, and we were uncertain if we could achieve the same level of quality as the native controls.

However, this styling issue wasn't our main concern. Avalonia simply lacks built-in support for web views. We could not find an Avalonia-based project that provides reliable web view support on every platform. As an alternative, we explored the use of [AvalonEdit](http://avalonedit.net/) to replace Monaco Editor. But again, it felt like a step backward compared to DevToys 1.0, which we wanted to avoid.

### WinUI 3 and WebView

DevToys 1.0 was a [UWP](https://learn.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide) app. While the programming language used is C# and XAML, the underlying runtime is not .NET, but [WinRT](https://en.wikipedia.org/wiki/Windows_Runtime). This can be seen as a Mac Catalyst equivalent but for Windows and Xbox (and the regretted Windows Phone). We couldn't use UWP for DevToys 2.0 as it doesn't allow us to run code dynamically in C#, a feature that .NET does provide.

The alternative was to use [WinUI 3 (WinApp SDK)](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/), which is more recent, comes with Fluent UI by default, and can be used in .NET. Unfortunately, we quickly encountered an issue in WinUI 3 that we didn't have in UWP: the web view does not support transparency. Here's why this was a problem for us:
1. Windows 11 introduced a new feature called [Mica](https://learn.microsoft.com/en-us/windows/apps/design/style/mica), a design material that renders the user's wallpaper in the app as a highly blurred texture. DevToys 1.0 uses this feature to make the app look more integrated with the system. To let this material render, the app's UI elements have to be semi-transparent. This means the WebView should also support a transparent background. This is supported in UWP, and even WPF, but [it is unfortunately not supported in WinUI 3](https://github.com/microsoft/microsoft-ui-xaml/issues/2992#issuecomment-670069746).
1. A consequence of it is that anywhere where we'd display the Monaco Editor, the Mica effect would not render. This would have been a regression compared to DevToys 1.0, and we really did not want that. While most customers may not care about this "detail" (and indeed, it is a detail), we do. You might think we are striving too much for perfection, and you might be right. But in our opinion, and based on the feedback we received, the appealing UI of DevToys is one of the reasons why people prefer it over alternative apps or websites. Therefore, we should not compromise on this aspect.

## Our solution

After six months of exploring various solutions, we were starting to lose hope. We began to think that we might never be able to build a cross-platform version of DevToys that would look and feel good on Windows, macOS, and Linux simultaneously.

Let's summarize the issues we encountered:
1. We couldn't find a satisfactory alternative to the Monaco Editor to avoid using a web view.
1. There's no XAML-based framework that provides a native look and feel on Windows, macOS, and Linux simultaneously.
1. Mac Catalyst apps have a significant issue with the keyboard on [WKWebView](https://developer.apple.com/documentation/webkit/wkwebview).
1. WinUI 3's WebView does not support transparency, which is a step back compared to DevToys 1.0.
1. .NET MAUI is not supported on Linux, at all.
1. Avalonia lacks built-in support for web view.

As much as we disliked it, we had to make a compromise. In our specific case, with our specific constraints on this project, creating a web-based app instead of a native app started to make sense. Should we have considered this option earlier? Probably. But we were very keen on creating a native app, knowing that people generally prefer them over web apps on desktop, and we were not ready to make this compromise initially.

### Electron

We wondered whether we should consider using Electron instead of .NET. Electron is a framework that primarily uses HTML, CSS and JavaScript to build cross-platform desktop applications. We were aware that Electron might not be the optimal choice in terms of performance and memory usage. However, we also recognized that it could enable us to create a cross-platform version of DevToys that would offer a pleasing look and feel on Windows, macOS, and Linux (with some efforts). We hesitated to use Electron for two reasons:
1. Electron applications are notorious for being heavy and bloated as each Electron application comes with its own copy of the Chromium web view and Node.js.
1. We were still not ready to give up on .NET and C#.

### Blazor Hybrid

After much discussion and debate, we decided to experiment with [Blazor Hybrid](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/?view=aspnetcore-8.0). Blazor is a framework for building interactive web UIs using C# and .NET instead of JavaScript. Blazor Hybrid allows the creation of desktop and mobile applications where the UI is powered by HTML, CSS and the logic is powered by .NET, running natively on the system. We believed this could be a good compromise considering our struggles until then. There were two points that attracted us compared to Electron:
1. We can utilize the web view installed on the operating system. There's no need to bloat our app with a copy of Chromium and Node.js like Electron does. It will use the Edge Web View (based on Chromium) on Windows, and WebKit on macOS and Linux.
1. The C# code runs natively on the operating system, outside of the web view's sandbox, which gives us excellent performance along with the ability to access all the system features.

### The Ultimate Argument for a Web-Based App

Another advantage of developing a web-based app instead of native one (whether it's Blazor Hybrid or another solution like Electron), in our specific case, is the memory consumption:

In DevToys 1.0, every time a user switched from one tool to another that required the Monaco Editor, the app was creating a new instance of the Web View, with a new instance of Monaco Editor inside. It was slow to load (sometimes taking 1-2 seconds before the editor appeared) and consumed a lot of memory. With an app where the whole UI is rendered through a web view, we won't need to load multiple of these web view. One is enough. And the Monaco Editor will be loaded once inside. This way, the memory consumption is reduced and the editor gets faster to load.

![Memory consumption difference between DevToys 1.0 and 2.0](blog/the-journey-to-devtoys-2.0/memory-consumption.png)

Of course, there are other ways to reduce memory consumption in DevToys as a native app, such as re-using the web view instances instead of creating a new one every time the user navigates from a tool to another, but this is one is by far the most efficient.

## Developing DevToys as a Blazor Hybrid app

### The User Interface

In our transition to a web-based user interface, we aimed to maintain a UI that closely mimics the native operating system. To achieve this, we used CSS and created numerous Blazor web components to emulate the Fluent UI on Windows, Aqua on macOS, and Yaru on Linux. We also used the system fonts and colors to make the app look and feel native on all platforms.

We initiated our work by reusing the CSS from [Fluent Svelte](https://fluent-svelte.vercel.app/), to replicate the Fluent UI as closely as possible. We then customized it to make it look like Aqua on macOS and Yaru on Linux. Although the UI on macOS and Linux may not appear as native as on Windows, we believe it's a reasonable compromise considering our primary user base is currently on Windows, and of course, we can iterate on the look and feel for macOS and Linux.

As of the DevToys developer, I ([Etienne Baudoux](https://www.linkedin.com/in/etiennebaudoux/)) have been away from CSS, TypeScript, and Blazor for quite some time. It felt like starting from scratch, and it took a few weeks to feel comfortable with it.

### On Windows

As previously mentioned, we encountered an issue with WinUI 3 where the web view does not support transparency. This was a significant hurdle for us, as we wanted to retain the Mica effect in DevToys. With Blazor Hybrid, we would have faced the same issue as long as we used WinUI 3.

Using UWP was not an option as it doesn't allow us to run code dynamically in C#, a feature that .NET provides, meaning that we would not be able to make DevToys support extensions.

As a consequence, we opted to use Blazor Hybrid with a trusty old [.NET WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/?view=netdesktop-8.0) host. This approach, coupled with a bit of Win32 interop, allowed us to apply the Mica effect using the Edge WebView, which supports transparency in WPF.
A [BlazorWebView](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.webview.wpf.blazorwebview?view=net-maui-8.0), a control that hosts a Blazor web app inside a native app, is available in WPF, which is handy for us.

### On macOS

We also mentioned earlier that on macOS specifically, Mac Catalyst apps have a significant issue with the keyboard and the WKWebView. We could have used Blazor Hybrid with a Mac Catalyst host, but we would have encountered the same problem.

Our alternative was to use, similar to the equivalent on Windows, a reliable [AppKit](https://developer.apple.com/documentation/appkit) host.
Like in WPF, .NET MAUI offers a BlazorWebView control that can be used in a Mac Catalyst app. Since .NET MAUI is open-source and under a permissive license (MIT), we were able to fork the [BlazorWebView control from .NET MAUI](https://github.com/dotnet/maui/blob/2e308dce7b707d57c1b0624c7a41d95c7b049c78/src/BlazorWebView/src/Maui/iOS/BlazorWebViewHandler.iOS.cs), which is relying on UIKit, and adapt it to AppKit.

A significant advantage of using AppKit over UIKit, which we discovered later in the development cycle, is the ability to use the [vibrancy effect](https://developer.apple.com/documentation/appkit/nsvisualeffectview#1674173). This gives DevToys a slightly more native-looking visual on macOS, thanks to the behind-window blending. It appears that Mac Catalyst apps don't support this feature.

### On Linux

On Linux, we used a [GTK](https://www.gtk.org/) host. We used the [gir.core](https://github.com/gircore/gir.core) as a GTK wrapper, primarily because it was the quickest one for us to get started with. We had no choice but to also fork the BlazorWebView control from .NET MAUI and adapt it to GTK. Although it was less straightforward than on macOS, we managed to make it work.

### Enabling extensibility

An important aspect of making an app extensible in .NET is to be able to load assemblies (`.dll` files for those unfamiliar with the term) dynamically. This means that the entry points of extensions should be discovered at runtime and can vary between app launches. Luckily, the [Managed Extensibility Framework (MEF)](https://docs.microsoft.com/en-us/dotnet/framework/mef/). is a .NET framework specifically designed to cater to this need. We chose to incorporate it into DevToys 2.0.

MEF offers the following pros and cons:
- **Pros**:
  - As a part of the .NET ecosystem, it is well-supported and maintained.
  - It's easy to use and provides a lot of flexibility.
  - It enables us to load assemblies dynamically, a critical feature for making DevToys extensible as users may install or uninstall an extension at any time.
- **Cons**:
  - Loading assemblies at startup can be time-consuming. This is why DevToys 2.0 takes longer to start than DevToys 1.0.
  - MEF heavily relies on [reflection](https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/reflection), which restricts the use of [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=net7%2Cwindows). This is another reason why DevToys 2.0 has a slower startup time.

In terms of startup performance, we have the opportunity to enhance it using [VS MEF](https://github.com/microsoft/vs-mef/blob/main/doc/why.md), a re-implementation of MEF utilized by Visual Studio for its own extensibility model. We could also potentially enable Native AOT on specific parts of the app that do not rely on MEF. This is a work that remains for us to do in an upcoming update.

## Conclusion

After a long journey, we finally released DevToys 2.0 on Windows, macOS, and Linux. We are thrilled to be able to offer DevToys to more developers and hope that this new version will make their lives easier. While our technical solutions to the challenges we encountered may not be the most optimal or straightforward, we take pride in our accomplishments and the knowledge we gained from the process.

Ultimately, our solution with Blazor Hybrid presents the following pros and cons:

- **Pros**:
  - Using CSS, we can imitate the Fluent UI on Windows, the UIKit on macOS and the Debian on Linux. As a result, DevToys appears and feels native on all platforms. We received feedback that, on Windows, it initially doesn't seem like the app is running in a web view.
  - The long-term memory consumption is lower than in DevToys 1.0, as there's only a single instance of the web view loaded.
  - We can leverage the operating system's installed web view, which makes the app more lightweight.
  - We can run C# code natively on the operating system, which allows us to take advantage of the system's features, such as the Taskbar's Jump List on Windows, or the app bar on macOS.
- **Cons**:
  - Maintaining the CSS to match the native look and feel of each platform will be time-consuming when platform's design language evolve.
  - The app starts slower than DevToys 1.0 because the web view has to be loaded at startup, and numerous assemblies are discovered at runtime.
  - We had to create a set of custom web components to ensure the UI looks good on every platform, which was quite time-consuming.
  - We now have to maintain our own implementation of the BlazorWebView on macOS and Linux, until we find potential alternative such as .

### Our recommendation for other .NET developers out there

Our reliance on the Monaco Editor and an extensibility model is somewhat unique, and likely doesn't mirror the needs of most developers. Consequently, while our journey may be intriguing, it should not be viewed as a definitive guide on crafting a cross-platform app with .NET.

Here are our recommendations based on our experience:
- For .NET developers creating a mobile app, consider using [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui) or [Uno Platform](https://platform.uno/).
  - If your app necessitates frequent display of a web-based UI, opt for [.NET MAUI Blazor Hybrid](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/maui?view=aspnetcore-8.0).
- For .NET developers creating a cross-platform desktop app without our constraints, consider using [Avalonia](https://avaloniaui.net/) or [Uno Platform](https://platform.uno/).
  - If you encounter constraints similar to ours, you now know that Blazor Hybrid can be a viable solution, and you can [use our code to achieve it](https://github.com/DevToys-app/DevToys).
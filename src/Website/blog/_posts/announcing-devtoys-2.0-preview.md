---
title: Announcing DevToys v2.0 Preview
description: Discover the redesigned DevToys, now on Windows, macOS and Linux.
category: News
date: 6/11/2024
gitHubAuthorName: veler
---

After more than 300,000 downloads of DevToys 1.0, we are thrilled to unveil the **DevToys v2.0.1 Preview** today. This version is the culmination of a year's worth of work, during which we completely rewrote the app.

## Windows, macOS and Linux

When we first launched DevToys, it was only available on Windows. Our decision to develop it for a single platform was influenced by the fact that we were 2 passionate developers, [Etienne Baudoux](https://www.linkedin.com/in/etiennebaudoux/) and [Benjamin Titeux](https://www.linkedin.com/in/benjamin-titeux/), who grew up with the Windows ecosystem and were comfortable with it. Moreover, similar apps already existed on macOS and Linux. However, the demand for macOS and Linux support quickly became our [most upvoted feature request on GitHub](https://github.com/DevToys-app/DevToys/issues/156).

Some amazing developers even started working on forks, often targeting only one platform. We often noticed that these versions were behind our Windows app in terms of features, tools and stability. So, in January 2023, we decided to rewrite DevToys with a focus on cross-platform support.

We're excited to announce that DevToys is now available on Windows, macOS and Linux! We hope that this will make DevToys more accessible to more developers.

![DevToys on macOS](blog/announcing-devtoys-2.0-preview/cross-platform.png)

## Introducing Extensions

On GitHub, we frequently receive suggestions for new tools to add to DevToys. As we develop DevToys in our spare time, we are often not going as fast as suggestions come. In addition, the use of XAML in DevToys 1.0 seemed to [deter potential contributors](https://github.com/DevToys-app/DevToys/issues/384#:~:text=in%20addition%20to%20the%20fact%20that%20I%20am%20not%20very%20skilled%20with%20xaml%20UI%20designs). We also aimed to keep DevToys streamlined and avoid bloating the app with too many tools.

To address this, we're introducing the concept of extensions in DevToys 2.0. Extensions are small plugins that anyone can develop to add new tools to DevToys. We hope this will accelerate the growth of DevToys and enable us to offer more tools to developers.

We've made an SDK available that doesn't require any knowledge of XAML and is very developer-friendly. [Read more to learn how to develop an extension for DevToys 2.0](https://devtoys.app/doc).

Starting today, the [PNG Compressor tool](https://www.nuget.org/packages?q=Tags%3A%22devtoys-app%22) is available as an extension. We chose not to include it by default with DevToys due to its different license and the ~4 MB of space it requires. This also gave us an opportunity to [demonstrate a real use case of the SDK](https://github.com/DevToys-app/DevToys.PngCompressor).

## Command-Line App

We also developed a command-line app for DevToys, allowing you to run DevToys from the terminal, and to use it in your CI/CD pipelines or any environment where there's no graphic user interface.

![DevToys CLI](blog/announcing-devtoys-2.0-preview/cli.png)

## Other improvements

DevToys 2.0 includes numerous other enhancements. Here are some of them:
- A new compact spacing option reduces the space between UI elements, catering to users who prefer this over the touch-friendly interface.
- Native support of Line Break in all tools. In addition, `Text Analyzer and Utilities` now includes a way to convert line breaks between Windows, Unix and Mac formats.
- New default tools, such as the `List Comparer`, `JSON Path tester`, `QR Code generator and reader`, and `JSON to CSV converter`. Existing tools have also received significant enhancements; for example, the `RegEx Tester` now includes a cheat sheet, and the `Date Converter` supports milliseconds, ticks and custom Epoch.
- The `Color Blind Simulator` now operates up to 2 times faster. Similarly, the [PNG Compressor tool](https://www.nuget.org/packages?q=Tags%3A%22devtoys-app%22) has been upgraded to offer both Lossy and Lossless compression, achieving image size reductions of up to 80%.
- The `Smart Detection` feature now allows for the chaining of tools, enabling the output of one tool to be transferred directly to another without the need for copy/pasting.

### The little details

- On Windows, favorite tools are also available in the Taskbar's Jump List.
- The search bar now supports fuzzy search, allowing you to find tools even if you make a typo.
- The app now has the option to automatically check for updates when it starts.
- The code editor has been updated to [Monaco Editor v0.39.0](https://github.com/microsoft/monaco-editor), adding some sweet features such as brace pair colorization.
- The `Color Blind Simulator` now operates up to 2 times faster.
- Memory consumption has been largely reduced when switching between tools. _We will detail how we achieved this in a future blog post._

## Closing words

DevToys 2.0 Preview is available today, and it wouldn't have been possible without our incredible community. We want to express our gratitude to all the developers who contributed to DevToys and helped us improve it. We hope you enjoy this new version and find it enhances your productivity.

We'd like to give special thanks to a few standout contributors:
- **Zee-Al-Eid Ahmad** @[zeealeid](https://x.com/zeealeid) for designing our fantastic new logo.
- **Kushal Azim Ekram** @[kforeverisback](https://github.com/kforeverisback), for helping us testing and packing DevToys for various Linux distributions.
- **Sou Niyari** @[niyari](https://github.com/niyari), for his valuable feedback and ideas since DevToys 1.0, his coding contributions and early feedback of 2.0.
- @[Antjac](https://github.com/Antjac) and @[sakana280](https://github.com/sakana280) for adding 2 new tools to DevToys 2.0, using this opportunity to test our extension SDK and provide feedback about it.
- All our translators for their invaluable help!
# TinyFoxDEBUG
这个一个TinyFox在VS2015上的调试工具 ；开发这个工具的初衷，是想更便捷的调试Nancy Web程序。
配置如下：
在VS2015菜单栏，选择Tools -> External Tools
新建一个项目：
Command: 调试器程序路径
Arguments: $(SolutionFileName)

在VS2015菜单栏，选择Tools -> Options -> Environment -> Keyboard
在Show commands containing:里键入 tools.externalcommand
根据序号选择你要设置快捷键的命令；设置 Press shortcut keys: 快捷键。

配置成功后就可以在VS2015里Build的项目后，再将项目Publish到Tinyfox的wwwroot目录里。

按快捷键试试，可以一键调试了。 如果Tinyfox可以命令行指定目录的话就可以真正的一键调试了。

maxzhang1985@gmail.com  梦奕飞翔

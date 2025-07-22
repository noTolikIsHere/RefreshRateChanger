Const SW_HIDE            =  0 ' Hides the window and activates another window.
Const SW_SHOWNORMAL      =  1 ' Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
Const SW_SHOWMINIMIZED   =  2 ' Activates the window and displays it as a minimized window.
Const SW_MAXIMIZE        =  3 ' Maximizes the specified window.
Const SW_SHOWMAXIMIZED   =  3 ' Activates the window and displays it as a maximized window.
Const SW_SHOWNOACTIVATE  =  4 ' Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
Const SW_SHOW            =  5 ' Activates the window and displays it in its current size and position.
Const SW_MINIMIZE        =  6 ' Minimizes the specified window and activates the next top-level window in the Z order.
Const SW_SHOWMINNOACTIVE =  7 ' Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
Const SW_SHOWNA          =  8 ' Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated.
Const SW_RESTORE         =  9 ' Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
Const SW_SHOWDEFAULT     = 10 ' Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
Const SW_FORCEMINIMIZE   = 11 ' Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.

Dim WindowStyle: WindowStyle = SW_HIDE

Dim Args()
Dim ArgCount: ArgCount = 0

Dim Index, Arg
For Index = 0 To WScript.Arguments.Count - 1
    Arg = WScript.Arguments(Index)
    If ArgCount = 0 And Left(Arg, 1) = "/" Then
        Arg = Replace(UCase(Mid(Arg, 2)), "_", "")
        If Left(Arg, 3) = "SW_" Then
            Arg = Mid(Arg, 3)
        End If
        Select Case Arg
            Case "HIDE"           : WindowStyle = SW_HIDE
            Case "SHOWNORMAL"     : WindowStyle = SW_SHOWNORMAL
            Case "SHOWMINIMIZED"  : WindowStyle = SW_SHOWMINIMIZED
            Case "MAXIMIZE"       : WindowStyle = SW_MAXIMIZE
            Case "SHOWMAXIMIZED"  : WindowStyle = SW_SHOWMAXIMIZED
            Case "SHOWNOACTIVATE" : WindowStyle = SW_SHOWNOACTIVATE
            Case "SHOW"           : WindowStyle = SW_SHOW
            Case "MINIMIZE"       : WindowStyle = SW_MINIMIZE
            Case "SHOWMINNOACTIVE": WindowStyle = SW_SHOWMINNOACTIVE
            Case "SHOWNA"         : WindowStyle = SW_SHOWNA
            Case "RESTORE"        : WindowStyle = SW_RESTORE
            Case "SHOWDEFAULT"    : WindowStyle = SW_SHOWDEFAULT
            Case "FORCEMINIMIZE"  : WindowStyle = SW_FORCEMINIMIZE
            Case Else
                Call MsgBox("Invalid windows style.", vbExclamation, WScript.ScriptName)
                Call WScript.Quit(1)
        End Select
    Else
        If InStr(Arg, " ") > 0 Then
            Arg = """" & Arg & """"
        End If
        ReDim Preserve Args(ArgCount)
        Args(ArgCount) = Arg
        ArgCount = ArgCount + 1
    End If
Next

If ArgCount = 0 Then
    Call MsgBox("Missing specification of program to launch.", vbExclamation, WScript.ScriptName)
    Call WScript.Quit(1)
End If

Dim CommandLine: CommandLine = Join(Args, " ")
Dim Shell: Set Shell = CreateObject("Wscript.Shell")
Call WScript.Quit(Shell.Run(CommandLine, WindowStyle, True))
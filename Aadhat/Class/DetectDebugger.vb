Imports System.Runtime.InteropServices

Public Class DetectDebugger
    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)>
    Private Shared Function CheckRemoteDebuggerPresent(ByVal hProcess As IntPtr, ByRef isDebuggerPresent As Boolean) As Boolean
    End Function
    Public Shared Sub Main()
        Dim isDebuggerPresent As Boolean = False
        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, isDebuggerPresent)
        Console.WriteLine("Debugger Attached: " & isDebuggerPresent)
        Console.ReadLine()
    End Sub
End Class
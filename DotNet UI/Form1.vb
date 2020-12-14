
Imports System.Runtime.InteropServices

<ComVisible(True)>
Public Class Form1

#Region "Interop"
    Delegate Sub RegisterCPPEventsCallback(events As CPPEventDefinitions)

    Public Structure CPPEventDefinitions
        Delegate Function Event1Delegate() As Double
        Public Event1 As Event1Delegate
        Delegate Function Event2Delegate() As Double
        Public Event2 As Event2Delegate
    End Structure

    Public Structure CSharpMethodsDefinitions
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Delegate Sub Function1Delegate(test As Double)
        Public Function1 As Function1Delegate
    End Structure

    <DllImport("CPP Library.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function Initialize(<MarshalAs(UnmanagedType.FunctionPtr)> RegisterCPPEvents As RegisterCPPEventsCallback, ByRef CSharpMethods As CSharpMethodsDefinitions) As Boolean : End Function
#End Region

    Public ReadOnly Property CPPEvents As CPPEventDefinitions

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim t As New Threading.Thread(Sub()
                                          Dim CSharpMethods As New CSharpMethodsDefinitions()
                                          CSharpMethods.Function1 = Sub(test As Double)
                                                                        MsgBox($"Callback from CPP: {test}")
                                                                    End Sub

                                          Initialize(Sub(events As CPPEventDefinitions)
                                                         _CPPEvents = events
                                                         MsgBox($"CPP Event 1 Returns: {CPPEvents.Event1()}")
                                                     End Sub, CSharpMethods)
                                      End Sub)
        t.IsBackground = True
        t.Start()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox($"CPP Event 2 Returns: {CPPEvents.Event2()}")
    End Sub
End Class

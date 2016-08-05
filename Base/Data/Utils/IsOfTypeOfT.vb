Namespace AcurSoft.Data.Utils


    Public Class IsOfType(Of T)
        Public ReadOnly Property Result As Boolean
        Public ReadOnly Property Value As T
        Public ReadOnly Property Self As IsOfType(Of T)


        Public Function CreateInstance() As T
            Return Activator.CreateInstance(Of T)()
        End Function

        Public Function CreateInstance(ParamArray args() As Object) As T
            Return DirectCast(Activator.CreateInstance(GetType(T), args), T)
        End Function

        Public Sub New(v As Object)
            _Self = Me
            If v.GetType.IsCastableTo(Of T) Then
                _Result = True
                _Value = DirectCast(v, T)
            End If
        End Sub


    End Class
End Namespace

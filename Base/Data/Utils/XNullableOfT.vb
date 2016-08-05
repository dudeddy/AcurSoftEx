Namespace AcurSoft.Data.Utils


    Public Class XNullable(Of T)
        Public ReadOnly Property HasValue As Boolean
        Public ReadOnly Property Value As T
        Public Sub New()
            Value = Nothing
        End Sub
        Public Sub New(v As T)
            Value = v
            HasValue = v IsNot Nothing
        End Sub
        Public Function IfNull(ifNullValue As T) As T
            If Me.HasValue Then Return Me.Value
            Return ifNullValue
        End Function

        Public Function IfNotNull(ifNotNullValue As T) As T
            If Me.HasValue Then Return Me.Value
            Return ifNotNullValue
        End Function


        Public Shared ReadOnly Property Empty() As XNullable(Of T)
            Get
                Return New XNullable(Of T)
            End Get
        End Property
        Public Shared Function Instance(v As T) As XNullable(Of T)
            Return New XNullable(Of T)(v)
        End Function

        Public Shared Sub IfNotNull(v As T, act As Action(Of T))
            If New XNullable(Of T)(v).HasValue Then
                act.Invoke(v)
            End If
        End Sub


        Public Shared Sub IfNull(v As T, act As Action(Of T))
            If Not New XNullable(Of T)(v).HasValue Then
                act.Invoke(v)
            End If
        End Sub
    End Class

End Namespace

Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace AcurSoft.Data.Filtering

    Public Class DateCriteriaElementBase(Of T)
        Public Property Value As T

        Private _Criteria As CriteriaOperator
        Public Overridable Property Criteria As CriteriaOperator
            Get
                If _Criteria Is Nothing Then
                    _Criteria = New OperandValue(Me.Value)
                End If
                Return _Criteria
            End Get
            Set(value As CriteriaOperator)
                _Criteria = value
            End Set
        End Property

        Public Function GetValueFromCriteria() As T
            Try
                _Value = DirectCast(New ExpressionEvaluator(New PropertyDescriptorCollection({}), Me.Criteria).Evaluate(Nothing), T)
                '_Criteria = New OperandValue(_Value)
            Catch ex As Exception
            End Try
            Return _Value
        End Function

        Public Sub New(v As T)
            Me.Value = v
        End Sub
        Public Sub New()
        End Sub
    End Class


End Namespace

Imports DevExpress.Data.Filtering
Namespace AcurSoft.Data.Filtering

    Public Class CriteriaFunctionBaseInfo
        Public Property Id As Integer
        Public Property FunctionOperatorType As FunctionOperatorType
        Public Property FunctionName As String
        Public Property Display As String
        Public Property Caption As String
        Public Property Criteria As CriteriaOperator
        Public Property XpoFunction As XpoFunctionBase

        Public Sub New()
        End Sub
        Public Sub New(xpoFunctionBase As XpoFunctionBase)
            Me.XpoFunction = xpoFunctionBase
            Me.Caption = xpoFunctionBase.Caption
            Me.FunctionName = xpoFunctionBase.Name
            Me.FunctionOperatorType = FunctionOperatorType.Custom
            Me.Display = xpoFunctionBase.Name
        End Sub
    End Class
End Namespace

Imports System.ComponentModel
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Data.Helpers
Imports DevExpress.XtraGrid.Views.Base

Public Class SumTop
    Implements ICustomFunctionOperator
    Public Const FunctionName As String = "SumTop"

    Public ReadOnly Property Name As String Implements ICustomFunctionOperator.Name
        Get
            Return "Ranked"
        End Get
    End Property

    Public Function Evaluate(ParamArray operands() As Object) As Object Implements ICustomFunctionOperator.Evaluate
        Throw New NotImplementedException()
    End Function

    Public Function ResultType(ParamArray operands() As Type) As Type Implements ICustomFunctionOperator.ResultType
        Return GetType(Object)
    End Function

    'Public Sub New(dataController As BaseListSourceDataController)
    '    MyBase.New(dataController, AggregateTypeEnum.Sum, True)
    'End Sub

    'Public Sub New(baseView As BaseView)
    '    Me.New(baseView.DataController)
    'End Sub

    'Public Overrides ReadOnly Property Name As String
    '    Get
    '        Return SumTop.FunctionName
    '    End Get
    'End Property
End Class

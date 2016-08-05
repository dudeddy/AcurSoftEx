Imports System.Reflection
Imports AcurSoft.Data.Filtering

Imports DevExpress.XtraEditors.Repository


Public Class XpoFunctionParamererFilterInfo
    Public ReadOnly Property XpoFunction As XpoFunctionBase
    Public ReadOnly Property ParameterInfo As ParameterInfo
    Public ReadOnly Property Index As Integer
    Public ReadOnly Property FilterIndex As Integer

    Private _EditorCreator As Func(Of RepositoryItem)
    Public Property EditorCreator As Func(Of RepositoryItem)
        Get
            If _EditorCreator Is Nothing Then
                _EditorCreator = Function() XpoFunctionFilterManager.GetEditorByType(Me.ParameterInfo.ParameterType)
            End If
            Return _EditorCreator
        End Get
        Set(value As Func(Of RepositoryItem))
            _EditorCreator = value
        End Set
    End Property
    Public Property ValueFixer As Func(Of Object, Object)


    Public Sub New(xpoFunction As XpoFunctionBase, parameterInfo As ParameterInfo, index As Integer)
        _XpoFunction = xpoFunction

        _ParameterInfo = parameterInfo
        _Index = index
        _FilterIndex = (index + 1) * 2
    End Sub

End Class

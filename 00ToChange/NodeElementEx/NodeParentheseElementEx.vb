Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering
    Public Enum NodeParentheseKindEnum
        Open
        Close
    End Enum

    Public Class NodeParentheseElementEx
        Inherits NodeEditableElementEx
        Public Overrides ReadOnly Property NodeKind As NodeElementKindEnum
        Public Overridable ReadOnly Property Parent As NodeEditableElementEx
        Public Overridable Property Opposite As NodeParentheseElementEx
        Public Overridable ReadOnly Property ParentheseKind As NodeParentheseKindEnum
        Public Const ParentheseOpenText As String = "( "
        Public Const ParentheseCloseText As String = " )"
        Public Sub New(parent As NodeEditableElementEx, Optional parentheseKind As NodeParentheseKindEnum = NodeParentheseKindEnum.Open)
            Me.New(parent.NodeEx, parentheseKind)
            Me.Parent = parent
        End Sub

        Public Sub New(ByVal node As ClauseNodeEx, Optional parentheseKind As NodeParentheseKindEnum = NodeParentheseKindEnum.Open)
            MyBase.New(node, ElementType.FieldAction, If(parentheseKind = NodeParentheseKindEnum.Open, ParentheseOpenText, ParentheseCloseText))
            Me.NodeKind = If(parentheseKind = NodeParentheseKindEnum.Open, NodeElementKindEnum.ParentheseOpen, NodeElementKindEnum.ParentheseClose)
            Me.ParentheseKind = parentheseKind
        End Sub
    End Class
End Namespace


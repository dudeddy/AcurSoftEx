Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Class NodeActionElementEx
        Inherits NodeEditableElementEx
        Public Const DefaultActionText As String = "@#"
        Public Const DefaultCollectionActionText As String = "@+"
        Public Const DefaultNodeRemoveText As String = "@-"
        Public Overridable ReadOnly Property Parent As NodeEditableElementEx

        Public Shared Function CreateActionElement(ByVal node As ClauseNodeEx, elementType As ElementType, text As String, Optional addToNode As Boolean = True) As NodeActionElementEx
            Dim elm As New NodeActionElementEx(node, elementType, text)
            If addToNode Then
                node.Elements.Add(elm)
            End If
            Return elm
        End Function

        Public Shared Function CreateNodeRemoveElement(ByVal node As ClauseNodeEx, Optional addToNode As Boolean = True) As NodeActionElementEx
            Return CreateActionElement(node, ElementType.NodeRemove, DefaultNodeRemoveText, addToNode)
        End Function

        Public Shared Function CreateCollectionActionElement(ByVal node As ClauseNodeEx, Optional addToNode As Boolean = True) As NodeActionElementEx
            Return CreateActionElement(node, ElementType.CollectionAction, DefaultCollectionActionText, addToNode)
        End Function

        Public Sub New(ByVal node As ClauseNodeEx, elementType As ElementType, text As String)
            MyBase.New(node, elementType, text)
        End Sub

        Public Sub New(ByVal parent As NodeEditableElementEx)
            MyBase.New(parent.NodeEx, ElementType.FieldAction, DefaultActionText)
            Me.ParentElement = parent
        End Sub
    End Class
End Namespace

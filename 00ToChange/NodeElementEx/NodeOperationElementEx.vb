Imports System.Reflection
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Class NodeItemCollectionElementEx
        Inherits NodeEditableElementEx
        Public Shared Function CreateItemCollectionElement(ByVal node As ClauseNodeEx, Optional addToNode As Boolean = True) As NodeItemCollectionElementEx
            Dim elm As New NodeItemCollectionElementEx(node)
            If addToNode Then
                node.Elements.Add(elm)
            End If
            Return elm
        End Function
        Public Sub New(ByVal node As ClauseNodeEx)
            MyBase.New(node, ElementType.ItemCollection, CStr(GetType(ClauseNode).InvokeMember("GetCollectionValuesString", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.InvokeMethod, Nothing, node, New Object() {})))
        End Sub
    End Class



    Public Class NodeOperationElementEx
        Inherits NodeEditableElementEx

        Public ReadOnly Property Operation As ClauseType

        Public Sub New(ByVal node As ClauseNodeEx, operation As ClauseType)
            MyBase.New(node, ElementType.Operation, node.ModelEx.GetMenuStringByType(operation))
            Me.Operation = operation
        End Sub

        Public Shared Function CreateOperationElement(ByVal node As ClauseNodeEx, operation As ClauseType, Optional addToNode As Boolean = True) As NodeOperationElementEx
            Dim elm As New NodeOperationElementEx(node, operation)
            If addToNode Then
                node.Elements.Add(elm)
            End If
            Return elm
        End Function

    End Class
End Namespace

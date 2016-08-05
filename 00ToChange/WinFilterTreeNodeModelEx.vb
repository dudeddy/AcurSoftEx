Imports System.Reflection
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.Data.Utils
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Utils.Frames
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.XtraEditors.Filtering
    Public Class WinFilterTreeNodeModelEx
        Inherits WinFilterTreeNodeModel

        Private _Labels As Dictionary(Of Node, FilterControlLabelInfo)
        Public Property Labels As Dictionary(Of Node, FilterControlLabelInfo)
            Get
                Return DirectCast(_LabelsFieldInfo.GetValue(Me), Dictionary(Of Node, FilterControlLabelInfo))
            End Get
            Set(value As Dictionary(Of Node, FilterControlLabelInfo))
                _LabelsFieldInfo.SetValue(Me, value)
            End Set
        End Property




        Private _LabelsFieldInfo As FieldInfo

        Public Sub New(ByVal control As FilterControl)
            MyBase.New(control)
            _LabelsFieldInfo = GetType(WinFilterTreeNodeModel).GetField("labels", BindingFlags.Instance Or BindingFlags.NonPublic)
        End Sub

        Public Overrides Sub OnVisualChange(action As FilterChangedActionInternal, node As Node)
            'MyBase.OnVisualChange(action, node)

            If action = FilterChangedActionInternal.NodeAdded Then
                Me.Labels(node) = New FilterControlLabelInfoEx(node)
            ElseIf action = FilterChangedActionInternal.RootNodeReplaced Then
                Me.Labels.Clear()
                'RecursiveVisitor(RootNode, Function(child) AnonymousMethod1(child, labels))
                RecursiveVisitor(RootNode, Sub(child)
                                               Dim info = New FilterControlLabelInfoEx(child)
                                               info.Clear()
                                               info.CreateLabelInfoTexts()
                                               Me.Labels(child) = info
                                           End Sub)
            Else
                MyBase.OnVisualChange(action, node)
            End If


        End Sub



        Public Overrides Function CreateClauseNode() As ClauseNode
            Return New ClauseNodeEx(Me)
        End Function

        Public Overrides Function ToCriteria(ByVal node As INode) As CriteriaOperator
            Return (New NodeToCriteriaProcessorEx(Me)).Process(node)
        End Function

        Public Overrides Function GetMenuStringByType(ByVal type As ClauseType) As String
            If type.IsDefined() Then
                Return MyBase.GetMenuStringByType(type)
            Else
                Dim info As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(type.IntegerValue)
                If info IsNot Nothing Then
                    Return info.Display
                End If
            End If
            Return MyBase.GetMenuStringByType(type)
        End Function

        Protected Overrides Function CreateNodeFromCriteria(ByVal criteria As CriteriaOperator) As Node
            Return DirectCast(CriteriaToTreeProcessorEx.GetTree(Me.CreateNodesFactory(), criteria, Nothing), Node)
        End Function

        Public Function GetBoundProperty(text As String) As XNullable(Of IBoundProperty)
            Dim propName As String = text.Trim("["c, "]"c).Trim().ToUpper
            Return XNullable(Of IBoundProperty).Instance(Me.FilterProperties.OfType(Of IBoundProperty).FirstOrDefault(Function(q) q.Name.ToUpper = propName))
        End Function
    End Class



End Namespace

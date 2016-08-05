Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.XtraEditors.Filtering

    Public Class FilterControlPainterEx
        Inherits FilterControlPainter

        Public ReadOnly Property FilterControlEx As FilterControlEx

        Public Sub New(ByVal filter As FilterControl)
            MyBase.New(filter)
        End Sub


        Public Sub New(ByVal filter As FilterControlEx)
            MyBase.New(filter)
            _FilterControlEx = filter
        End Sub

        Protected Overrides Sub DrawNodeLabel(ByVal node As Node, ByVal info As ControlGraphicsInfoArgs)
            MyBase.DrawNodeLabel(node, info)
            If node.Elements(0).ElementType <> ElementType.Group Then
                Dim labelInfo As FilterControlLabelInfo = (TryCast(node.Model, WinFilterTreeNodeModel))(node)
                'labelInfo.ViewInfo.
                Dim vis As New List(Of FilterLabelInfoTextViewInfo)
                For i As Integer = 0 To labelInfo.ViewInfo.Count - 1
                    vis.Add(TryCast(labelInfo.ViewInfo.Item(i), FilterLabelInfoTextViewInfo))
                Next
                Dim qry As List(Of FilterLabelInfoTextViewInfo) = (
                From q In node.Elements, l In vis
                Where l.InfoText.Tag Is q AndAlso q.Text = ClauseNodeEx.FunctionsButtonTag
                Select l).ToList
                For Each lvi In qry
                    With lvi.ItemBounds
                        Dim p As New Point(.X, .Y + (.Height - Ressources.functions_16x16.Height) \ 2)
                        info.Graphics.DrawImage(Ressources.functions_16x16, p)
                    End With
                Next


                'Dim lst As List(Of NodeEditableElement) = node.Elements.Where (Function(q) TryCast(labelInfo.ViewInfo.Item(q.Index + 2), FilterLabelInfoTextViewInfo))
                'For Each elm In node.Elements
                '    'If elm.ElementType = ElementType.FieldAction AndAlso elm.Text = "@~" Then
                '    If elm.ElementType.value__ = FieldActionEx.ShowFunctions.value__ Then
                '        If labelInfo.ViewInfo.Count > elm.Index + 1 Then

                '            'Dim foo As NodeEditableElement = node.Elements(lviIndex - 1)
                '            ''While foo.Text <> "@#" OrElse foo.Text <> "@-"
                '            'While foo.IsValueElement
                '            '    lviIndex -= 1
                '            '    foo = node.Elements(lviIndex)
                '            'End While
                '            Dim lviIndex As Integer = elm.Index
                '            Dim lvi As FilterLabelInfoTextViewInfo = TryCast(labelInfo.ViewInfo.Item(lviIndex), FilterLabelInfoTextViewInfo)
                '            While lvi.TextElement.Text <> "     "
                '                lviIndex += 1
                '                lvi = TryCast(labelInfo.ViewInfo.Item(lviIndex), FilterLabelInfoTextViewInfo)
                '                'foo = node.Elements(lviIndex)
                '            End While

                '            'lviIndex += 2

                '            'Dim xzz = node.Elements(lviIndex)
                '            'Dim lvi As FilterLabelInfoTextViewInfo = TryCast(labelInfo.ViewInfo.Item(lviIndex + 1), FilterLabelInfoTextViewInfo)
                '            With lvi.ItemBounds
                '                Dim p As New Point(.X, .Y + (.Height - Ressources.functions_16x16.Height) \ 2)
                '                'lvi.InfoText.Color = Color.Transparent
                '                info.Graphics.DrawImage(Ressources.functions_16x16, p)
                '            End With
                'End If
                'End If
                '    Next
                'labelInfo.
            End If
            'MyBase.DrawNodeLabel(node, info)

        End Sub

    End Class
End Namespace

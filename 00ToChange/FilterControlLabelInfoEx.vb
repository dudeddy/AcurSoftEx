
Imports AcurSoft.Data.Filtering
Imports DevExpress.Utils.Frames
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.XtraEditors.Filtering

    Public Class FilterControlLabelInfoEx
        Inherits FilterControlLabelInfo
        Private _OldparentheseElement As NodeParentheseElementEx
        Private _OldparentheseElementOpposite As NodeParentheseElementEx
        Public Sub New(ByVal node As Node)
            MyBase.New(node)
        End Sub

        Public Function GetLabelInfoTexts() As List(Of LabelInfoText)
            Return Me.Texts.OfType(Of LabelInfoText).ToList
        End Function
        Public Sub ResetParentheses()
            Me.ResetParentheses(Me.GetLabelInfoTexts().Where(Function(q) q.Tag IsNot Nothing AndAlso TypeOf q.Tag Is NodeParentheseElementEx).ToList)
        End Sub

        Public Sub ResetParentheses(vis As List(Of LabelInfoText))
            If vis.Count > 0 AndAlso _OldparentheseElement IsNot Nothing Then
                vis.FirstOrDefault(Function(q) q.Tag Is _OldparentheseElement).Color = Color.Black
                vis.FirstOrDefault(Function(q) q.Tag Is _OldparentheseElementOpposite).Color = Color.Black
            End If
            _OldparentheseElement = Nothing
            _OldparentheseElementOpposite = Nothing
        End Sub


        Public Sub ActivateParenthese(p As NodeParentheseElementEx)
            Dim vis As List(Of LabelInfoText) = Me.GetLabelInfoTexts().Where(Function(q) q.Tag IsNot Nothing AndAlso TypeOf q.Tag Is NodeParentheseElementEx).ToList
            Dim parentheseTextElement As LabelInfoText = vis.FirstOrDefault(Function(q) q.Tag Is p)
            Dim parentheseElementOpposite As NodeParentheseElementEx = p.Opposite
            Dim parentheseTextElementOpposite As LabelInfoText = vis.FirstOrDefault(Function(q) q.Tag Is parentheseElementOpposite)
            parentheseTextElement.Color = Color.Red
            parentheseTextElementOpposite.Color = Color.Red
            Me.ResetParentheses(vis)
            _OldparentheseElement = p
            _OldparentheseElementOpposite = parentheseElementOpposite
        End Sub





        'Public Overrides Sub Paint(ByVal info As ControlGraphicsInfoArgs)

        '    'ViewInfo.Calculate(info.Graphics)
        '    'ViewInfo.TopLine = 0
        '    'Dim isParenthesePressed As Boolean = False
        '    'Dim parentheseElement As NodeParentheseElementEx = Nothing
        '    'Dim parentheseElementOpposite As NodeParentheseElementEx = Nothing
        '    'Dim parentheseTextElement As FilterLabelInfoTextViewInfo = Nothing
        '    'Dim parentheseTextElementOpposite As FilterLabelInfoTextViewInfo = Nothing
        '    'Dim oppositeParentheseProcessed As Boolean = False

        '    'Dim vis As New List(Of FilterLabelInfoTextViewInfo)
        '    'For i As Integer = 0 To Me.ViewInfo.Count - 1
        '    '    Dim textViewInfo As FilterLabelInfoTextViewInfo = CType(Me.ViewInfo(i), FilterLabelInfoTextViewInfo)
        '    '    vis.Add(textViewInfo)
        '    '    Dim nodeElement As NodeEditableElement = TryCast(textViewInfo.InfoText.Tag, NodeEditableElement)
        '    '    If TypeOf nodeElement Is NodeParentheseElementEx Then
        '    '        If textViewInfo.ViewInfo.IsPressed Then
        '    '            isParenthesePressed = True
        '    '            parentheseElement = DirectCast(nodeElement, NodeParentheseElementEx)
        '    '            parentheseElementOpposite = parentheseElement.Opposite
        '    '            parentheseTextElement = textViewInfo
        '    '        End If
        '    '    End If
        '    'Next
        '    'If isParenthesePressed Then
        '    '    parentheseTextElementOpposite = vis.FirstOrDefault(Function(q) q.InfoText.Tag Is parentheseElementOpposite)
        '    '    parentheseTextElementOpposite.InfoText.Color = Color.Red
        '    '    parentheseTextElement.InfoText.Color = Color.Red
        '    'End If


        '    'For i As Integer = 0 To ViewInfo.Count - 1
        '    '    Dim textViewInfo As FilterLabelInfoTextViewInfo = CType(ViewInfo(i), FilterLabelInfoTextViewInfo)
        '    '    ViewInfo(i).Draw(info.Cache, info.ViewInfo.Appearance.GetFont(), textViewInfo.InfoText.Color, info.ViewInfo.Appearance.GetStringFormat())
        '    'Next i
        '    MyBase.Paint(info)
        '    'isParenthesePressed = False
        'End Sub
    End Class

End Namespace

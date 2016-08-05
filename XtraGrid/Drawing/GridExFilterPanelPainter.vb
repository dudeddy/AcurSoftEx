Imports AcurSoft.XtraGrid.Views.Grid
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Drawing

    Public Class GridExFilterPanelPainter
        Inherits SkinGridFilterPanelPainter
        Public Property View() As GridViewEx
        Private customButtonPainter As SkinEditorButtonPainter

        Public Sub New(ByVal provider As GridView)
            MyBase.New(provider)
            View = TryCast(provider, GridViewEx)
            customButtonPainter = New SkinEditorButtonPainter(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel)
        End Sub
        Protected Overrides Function CalcButtonLocation(ByVal client As Rectangle, ByVal size As Size, ByVal isRight As Boolean) As Point
            Dim point As Point = MyBase.CalcButtonLocation(client, size, isRight)
            If point.X < 10 Then
                point.X += View.UpdateFilterPanelButtonsRects() ' set the indent
            End If

            Return point
        End Function

        Public Overrides Sub DrawObject(ByVal e As ObjectInfoArgs)
            MyBase.DrawObject(e)
            DrawCustomButtons(e.Cache)
        End Sub

        Private Sub DrawCustomButtons(ByVal cache As GraphicsCache)
            For Each button As EditorButton In View.FilterPanelButtonsStore.Keys
                customButtonPainter.DrawObject(GetButtonInfoArgs(cache, View.FilterPanelButtonsStore(button), button))
            Next button
        End Sub

        Friend Function GetButtonInfoArgs(ByVal cache As GraphicsCache, ByVal args As EditorButtonObjectInfoArgs, ByVal button As EditorButton) As EditorButtonObjectInfoArgs
            args.Cache = cache
            Dim state As ObjectState = ObjectState.Normal
            If TypeOf button.Tag Is ObjectState Then
                state = CType(button.Tag, ObjectState)
            End If
            args.State = state
            Return args
        End Function
    End Class
End Namespace


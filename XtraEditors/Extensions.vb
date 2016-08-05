Imports System.Runtime.CompilerServices
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo

Namespace AcurSoft.XtraEditors

    Partial Module Extensions
        <Extension>
        Public Sub ShowMenu(ByVal editor As ButtonEdit, button As EditorButton, menu As DXPopupMenu)
            Dim bvi As EditorButtonObjectInfoArgs = DirectCast(editor.GetViewInfo(), ButtonEditViewInfo).ButtonInfoByButton(button)
            DevExpress.Utils.Menu.MenuManagerHelper.ShowMenu(menu, editor.LookAndFeel, Nothing, editor, New Point(bvi.Bounds.Left, bvi.Bounds.Bottom))
        End Sub
    End Module
End Namespace

Imports System.ComponentModel
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.XtraEditors.Filtering

    Public Class PopupCustomFunctionsMenuShowingEventArgs
        Inherits CancelEventArgs
        'Inherits PopupMenuShowingEventArgs

        Public ReadOnly Property CurrentNode As Node
        'Public ReadOnly Property FocusedElementType As ElementType
        'Public ReadOnly Property FieldActionEx As FieldActionEx
        Public ReadOnly Property Menu As DXPopupMenu
        Public Property Point As Point
        'Public Sub New(node As Node, menu As DXPopupMenu, p As Point)
        '    _CurrentNode = node
        '    _Menu = menu
        '    _Point = p
        'End Sub
        Public Sub New(node As Node, p As Point)
            _CurrentNode = node
            _Menu = New DXPopupMenu
            _Point = p
        End Sub
    End Class
End Namespace

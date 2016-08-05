Imports DevExpress.Skins
Imports DevExpress.XtraGrid.Views.Grid
'Imports System.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Utils
Imports System.ComponentModel
Imports AcurSoft.XtraGrid.Columns
Imports AcurSoft.XtraGrid.Views.Grid
Imports AcurSoft.XtraGrid.Columns.Helpers
Namespace AcurSoft.XtraGrid.Views.Grid.Extenders

    Public Class GridViewColumnsSelectionHelper
        Inherits Component
        Public Const CheckBoxID As String = "CheckBoxID"
        Public Const CheckBoxWidth As Integer = 14


        Private _GridView As GridViewEx = Nothing
        Private ReadOnly _CheckBoxSize As Size
        'Public Property DrawCheckBoxByDefault() As Boolean
        Public Property DrawCheckBoxMode As DrawCheckBoxModeEnum = DrawCheckBoxModeEnum.Never
        Private _InHeader As Boolean = False
        Private _Column As GridExColumn = Nothing
        'Private _IsCheckBoxCollectionInitialized As Boolean = False
        Private _CheckBoxCollection As ImageCollection = Nothing
        Private ReadOnly Property CheckBoxCollection As ImageCollection
            Get
                If _CheckBoxCollection Is Nothing Then
                    Dim skin As Skin = EditorsSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default)
                    Dim skinElement As SkinElement = skin("CheckBox")
                    If skinElement Is Nothing Then Return Nothing
                    _CheckBoxCollection = skinElement.Image.GetImages()
                End If
                Return _CheckBoxCollection
            End Get
        End Property

        Public Sub New()
            _CheckBoxSize = New Size(GridViewColumnsSelectionHelper.CheckBoxWidth, GridViewColumnsSelectionHelper.CheckBoxWidth)
            'DrawCheckBoxByDefault = True
            DrawCheckBoxMode = DrawCheckBoxModeEnum.Never
        End Sub

        Public Sub New(view As GridViewEx)
            Me.New()
            Me.View = view
        End Sub

        <Browsable(False)>
        Public Property View() As GridViewEx
            Get
                Return _GridView
            End Get
            Set(ByVal value As GridViewEx)
                If _GridView Is value Then
                    Return
                End If
                OnViewChanging()
                _GridView = value
                OnViewChanged()
            End Set
        End Property

        Protected Overridable Sub OnViewChanging()
            ViewEvents(False)
        End Sub

        Protected Overridable Sub OnViewChanged()
            ViewEvents(True)
        End Sub

        Private Sub OnMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
            If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return
            If Not ColumnHeaderContainsCursor(e.Location) Then Return
            If CheckBoxContainsCursor(e.Location, _Column) Then
                ResetChecked(_Column)
                SetCheckBoxState(_Column, ObjectState.Normal)
                DXMouseEventArgs.GetMouseArgs(e).Handled = True
            End If
        End Sub

        Private Sub OnMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return
            If Not ColumnHeaderContainsCursor(e.Location) Then Return
            Dim state As ObjectState = ObjectState.Normal
            If CheckBoxContainsCursor(e.Location, _Column) Then
                state = ObjectState.Hot
            End If
            SetCheckBoxState(_Column, state)
        End Sub

        Private Sub OnMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
            If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return
            If Not ColumnHeaderContainsCursor(e.Location) Then
                Return
            End If
            If CheckBoxContainsCursor(e.Location, _Column) Then
                SetCheckBoxState(_Column, ObjectState.Pressed)
            End If
        End Sub

        Private Sub view_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return
            'If Not DrawCheckBoxByDefault AndAlso _Column IsNot Nothing Then
            If DrawCheckBoxMode <> DrawCheckBoxModeEnum.Always AndAlso _Column IsNot Nothing Then
                _InHeader = False
                _GridView.InvalidateColumnHeader(_Column)
            End If
        End Sub

        Private Sub ResetChecked(ByVal col As GridExColumn)
            Dim temp As ColumnStateRepository = _Column.CheckedStateRepository ' GetColumnStateRepository(_Column)
            temp.Checked = Not temp.Checked
            RaiseColumnCheckedChanged(New ColumnCheckedChangedEventArgs(col, temp.Checked))
        End Sub

        Private Sub SetCheckBoxState(ByVal column As GridExColumn, ByVal state As ObjectState)
            'GetColumnStateRepository(column).State = state
            column.CheckedStateRepository.State = state
            _GridView.InvalidateColumnHeader(column)
        End Sub

        Private Function ColumnHeaderContainsCursor(ByVal pt As Point) As Boolean
            Dim hitInfo As GridHitInfo = _GridView.CalcHitInfo(pt)
            _Column = DirectCast(hitInfo.Column, GridExColumn)
            _InHeader = hitInfo.HitTest = GridHitTest.Column
            Return _InHeader
        End Function

        Private Function CheckBoxContainsCursor(ByVal point As Point, ByVal col As GridColumn) As Boolean
            Dim rect As Rectangle = CalcCheckBoxRectangle(col)
            Return rect.Contains(point)
        End Function

        Private Function CalcCheckBoxRectangle(ByVal col As GridColumn) As Rectangle
            Dim info As New GraphicsInfo()
            info.AddGraphics(Nothing)
            Dim viewInfo As GridViewInfo = TryCast(_GridView.GetViewInfo(), GridViewInfo)
            Dim columnArgs As GridColumnInfoArgs = viewInfo.ColumnsInfo(col)
            Dim rect As Rectangle = GetCheckBoxRectangle(columnArgs, info.Graphics)
            info.ReleaseGraphics()
            Return rect
        End Function

        Private Function GetCheckBoxRectangle(ByVal columnArgs As GridColumnInfoArgs, ByVal gr As Graphics) As Rectangle
            If columnArgs Is Nothing Then Return New Rectangle
            Dim columnRect As Rectangle = columnArgs.Bounds
            Dim innerElementsWidth As Integer = CalcInnerElementsMinWidth(columnArgs, gr)
            Dim Rect As New Rectangle(columnRect.Right - innerElementsWidth - _CheckBoxSize.Width - 5, columnRect.Y + columnRect.Height \ 2 - _CheckBoxSize.Height \ 2, _CheckBoxSize.Width, _CheckBoxSize.Height)
            Return Rect
        End Function

        Private Function CalcInnerElementsMinWidth(ByVal columnArgs As GridColumnInfoArgs, ByVal gr As Graphics) As Integer
            Dim canDrawMode As Boolean = True
            Return columnArgs.InnerElements.CalcMinSize(gr, canDrawMode).Width
        End Function

        Private Sub OnCustomDrawColumnHeader(ByVal sender As Object, ByVal e As ColumnHeaderCustomDrawEventArgs)
            If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return
            If e.Column Is Nothing Then Return
            DefaultDrawColumnHeader(e)

            If CanDrawCheckBox(DirectCast(e.Column, GridExColumn)) Then
                DrawCheckBox(e)
            End If

            e.Handled = True
        End Sub

        'Private Function GetColumnStateRepository(ByVal col As GridColumn) As ColumnStateRepository
        '    Dim dic As Dictionary(Of String, Object) = Nothing
        '    If col.Tag Is Nothing Then
        '        dic = New Dictionary(Of String, Object)
        '        Dim columnStateRepository As New ColumnStateRepository With {.State = ObjectState.Normal, .Checked = False}
        '        dic.Add(GridViewColumnHeaderExtender.CheckBoxID, columnStateRepository)
        '        col.Tag = dic
        '        Return columnStateRepository
        '    ElseIf TypeOf col.Tag Is Dictionary(Of String, Object)
        '        dic = DirectCast(col.Tag, Dictionary(Of String, Object))
        '        If dic.ContainsKey(GridViewColumnHeaderExtender.CheckBoxID) Then
        '            Return DirectCast(dic(GridViewColumnHeaderExtender.CheckBoxID), ColumnStateRepository)
        '        Else
        '            Dim columnStateRepository As New ColumnStateRepository With {.State = ObjectState.Normal, .Checked = False}
        '            dic.Add(GridViewColumnHeaderExtender.CheckBoxID, columnStateRepository)
        '            col.Tag = dic
        '            Return columnStateRepository
        '        End If
        '    Else
        '        Throw New Exception
        '    End If
        '    'If col.Tag IsNot Nothing AndAlso TypeOf col.Tag Is Dictionary(Of String, Object) Then
        '    '    If Not DirectCast(col.Tag, Dictionary(Of String, Object)).ContainsKey(GridViewColumnHeaderExtender.CheckBoxID) Then
        '    '        '    Return
        '    '        'Else
        '    '        DirectCast(col.Tag, Dictionary(Of String, Object)).Add(GridViewColumnHeaderExtender.CheckBoxID, New ColumnStateRepository With {.State = ObjectState.Normal, .Checked = False})
        '    '    End If
        '    'Else
        '    '    Dim dic = New Dictionary(Of String, Object)
        '    '    dic.Add(GridViewColumnHeaderExtender.CheckBoxID, New ColumnStateRepository With {.State = ObjectState.Normal, .Checked = False})
        '    '    col.Tag = dic
        '    'End If

        '    'col.Tag = New ColumnStateRepository With {
        '    '    .State = ObjectState.Normal,
        '    '    .Checked = False
        '    '}
        'End Function

        Private Function CanDrawCheckBox(ByVal col As GridExColumn) As Boolean
            'If DrawCheckBoxMode = DrawCheckBoxModeEnum.Never Then Return

            Select Case DrawCheckBoxMode
                Case DrawCheckBoxModeEnum.Never
                    Return False
                Case DrawCheckBoxModeEnum.Always
                    Return True
                Case DrawCheckBoxModeEnum.OnHoverOnly
                    Return _InHeader AndAlso col Is _Column
                Case DrawCheckBoxModeEnum.IsCheckedAndOnHover
                    If _InHeader AndAlso col Is _Column Then 'OnHover
                        Return True
                    End If
                    'Return col IsNot Nothing AndAlso col.Tag IsNot Nothing AndAlso TypeOf col.Tag Is Dictionary(Of String, Object) AndAlso DirectCast(col.Tag, Dictionary(Of String, Object)).ContainsKey(GridViewColumnHeaderExtender.CheckBoxID) AndAlso GetColumnStateRepository(col).Checked
                    '    Return False
                    Return col.CheckedStateRepository.Checked
                    'Return GetColumnStateRepository(col).Checked
                    'End If


                    'Return col IsNot Nothing AndAlso col.Tag IsNot Nothing AndAlso TypeOf col.Tag Is ColumnStateRepository AndAlso DirectCast(col.Tag, ColumnStateRepository).Checked
            End Select

            'Return DrawCheckBoxByDefault OrElse (inHeader AndAlso col Is column)
            'Return (col IsNot Nothing AndAlso col.Tag IsNot Nothing AndAlso TypeOf col.Tag Is ColumnStateRepository AndAlso TryCast(col.Tag, ColumnStateRepository).Checked) OrElse (_InHeader AndAlso col Is _Column)
            'Try

            'Catch ex As Exception
            '    Dim x = 0
            'End Try
            Return False
        End Function

        Private Sub DrawCheckBox(ByVal e As ColumnHeaderCustomDrawEventArgs)
            Dim index As Integer = 0

            Dim temp As ColumnStateRepository = DirectCast(e.Column, GridExColumn).CheckedStateRepository '  GetColumnStateRepository(e.Column)
            Dim offset As Integer = If(temp.Checked = True, 4, 0)
            Select Case temp.State
                Case ObjectState.Normal
                    index = offset
                Case ObjectState.Hot
                    index = offset + 1
                Case ObjectState.Hot Or ObjectState.Pressed
                    index = offset + 2
            End Select
            Dim rect As Rectangle = CalcCheckBoxRectangle(e.Column)
            'CheckCheckBoxCollection()
            e.Graphics.DrawImage(Me.CheckBoxCollection.Images(index), rect)
        End Sub



        'Protected Overridable Function GetCheckBoxImages() As ImageCollection
        'End Function

        Private Sub DefaultDrawColumnHeader(ByVal e As ColumnHeaderCustomDrawEventArgs)
            e.Painter.DrawObject(e.Info)
        End Sub

        Private Sub ViewEvents(ByVal subscribe As Boolean)
            If _GridView Is Nothing Then
                Return
            End If
            If Not subscribe Then
                RemoveHandler _GridView.CustomDrawColumnHeader, AddressOf OnCustomDrawColumnHeader
                RemoveHandler _GridView.MouseDown, AddressOf OnMouseDown
                RemoveHandler _GridView.MouseUp, AddressOf OnMouseUp
                RemoveHandler _GridView.MouseMove, AddressOf OnMouseMove
                RemoveHandler _GridView.MouseLeave, AddressOf view_MouseLeave
                Return
            End If

            AddHandler _GridView.CustomDrawColumnHeader, AddressOf OnCustomDrawColumnHeader
            AddHandler _GridView.MouseDown, AddressOf OnMouseDown
            AddHandler _GridView.MouseUp, AddressOf OnMouseUp
            AddHandler _GridView.MouseMove, AddressOf OnMouseMove
            AddHandler _GridView.MouseLeave, AddressOf view_MouseLeave
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            ViewEvents(False)
            MyBase.Dispose(disposing)
        End Sub

        Public Event ColumnCheckedChanged As EventHandler(Of ColumnCheckedChangedEventArgs)
        Public Overridable Sub RaiseColumnCheckedChanged(ByVal ea As ColumnCheckedChangedEventArgs)
            Dim handler As EventHandler(Of ColumnCheckedChangedEventArgs) = ColumnCheckedChangedEvent
            If handler IsNot Nothing Then
                handler(_GridView, ea)
            End If
        End Sub

    End Class
End Namespace

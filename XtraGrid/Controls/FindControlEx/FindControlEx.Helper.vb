Imports DevExpress.Utils.Paint
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.ComponentModel
Imports System.Reflection
Imports System.IO
Imports DevExpress.Data.Filtering
Namespace AcurSoft.XtraGrid.Controls

    Partial Public Class FindControlEx
        Private _Timer As Timer

        Public Sub Init()
            _Timer = New Timer()
            _Timer.Interval = _ViewEx.OptionsFind.FindDelay
            AddHandler _Timer.Tick, AddressOf timer_Tick

            SubscibeViewEvent(False)
            SubscibeViewEvent(True)

            SubscribeRIEvent(False)
            _SearchEditor = Me.FindEdit
            ResetActiveRI()

        End Sub



        Public ReadOnly Property SearchEditor As ButtonEdit
        'Private _GridControl As GridControlEx
        'Private _TargetView As GridViewEx
        Private _ShowResultButton As EditorButton
        Private _ShowAdvancedFilterButton As EditorButton
        Private _FilterCellText As String = String.Empty
        Private _FindList As New Dictionary(Of GridCell, Boolean)()

        Private ReadOnly Property FindListIsEmpty() As Boolean
            Get
                Return _FindList.Keys.Count = 0
            End Get
        End Property

        Private Sub ResetActiveRI()
            If DesignMode Then Return
            '_SearchEditor.Properties.Buttons.Clear()
            _ShowResultButton = New EditorButton(ButtonPredefines.Glyph, "0 of 0", 50, False, True, False, ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), Nothing, "", Nothing, Nothing, True)
            _ShowAdvancedFilterButton = New EditorButton(ButtonPredefines.Glyph) With {
                .ImageLocation = ImageLocation.MiddleCenter}
            If _ViewEx.OptionsView.ShowXtraPanel Then
                _ShowAdvancedFilterButton.Image = Ressources.filter_delete_16x16
                _ShowAdvancedFilterButton.ToolTip = "Hide Advanced Filter."

            Else
                _ShowAdvancedFilterButton.Image = Ressources.filter_16x16
                _ShowAdvancedFilterButton.ToolTip = "Show Advanced Filter."
            End If


            _SearchEditor.Properties.Buttons.AddRange(New EditorButton() {
                New EditorButton(ButtonPredefines.Search),
                New EditorButton(ButtonPredefines.Clear),
                New EditorButton(ButtonPredefines.SpinLeft),
                New EditorButton(ButtonPredefines.SpinRight),
                _ShowResultButton,
                _ShowAdvancedFilterButton
            })
            SubscribeRIEvent(True)
        End Sub

        Private Sub SubscribeRIEvent(ByVal subscribe As Boolean)
            If _SearchEditor Is Nothing Then Return
            RemoveHandler _SearchEditor.Properties.ButtonClick, AddressOf ButtonClick
            RemoveHandler _SearchEditor.Properties.KeyUp, AddressOf Editor_KeyUp
            RemoveHandler _SearchEditor.Properties.KeyDown, AddressOf Editor_KeyDown
            If subscribe Then
                AddHandler _SearchEditor.Properties.KeyUp, AddressOf Editor_KeyUp
                AddHandler _SearchEditor.Properties.KeyDown, AddressOf Editor_KeyDown
                AddHandler _SearchEditor.Properties.ButtonClick, AddressOf ButtonClick
            End If
        End Sub

        Private Sub Editor_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            _Timer.Stop()
            Select Case e.KeyData
                Case Keys.Enter
                    _SearchEditor.PerformClick(_SearchEditor.Properties.Buttons(0))
                Case (Keys.Control Or Keys.Left)
                    _SearchEditor.PerformClick(_SearchEditor.Properties.Buttons(1))
            End Select
        End Sub


        Private Sub Editor_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
            _Timer.Start()
        End Sub


        'Private Sub ResetActiveGrid()

        '    SubscibeViewEvent(True)
        'End Sub

        Private Sub SubscibeViewEvent(ByVal subscribe As Boolean)
            If _ViewEx Is Nothing Then Return
            RemoveHandler _ViewEx.CustomDrawCell, AddressOf CustomDrawCell
            If subscribe Then
                AddHandler _ViewEx.CustomDrawCell, AddressOf CustomDrawCell
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            SubscibeViewEvent(False)
            SubscribeRIEvent(False)
            MyBase.Dispose(disposing)
        End Sub

        Private Sub CustomDrawCell(ByVal sender As Object, ByVal e As RowCellCustomDrawEventArgs)
            If FindListIsEmpty Then Return
            Dim filterTextIndex = -1 'Then Return
            Dim filterCellText As String = _FilterCellText
            Dim displayText As String = e.DisplayText
            If _ViewEx.OptionsFilter.AccentInsensitive Then
                If Not String.IsNullOrEmpty(displayText) Then
                    displayText = RemoveDiacriticsFunction.Instance.Evaluate(e.DisplayText).ToString
                End If
                filterCellText = RemoveDiacriticsFunction.Instance.Evaluate(filterCellText).ToString
            End If
            filterTextIndex = displayText.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase)

            If filterTextIndex = -1 Then Return

            Dim temp As New GridCell(e.RowHandle, e.Column)
            If _ViewEx.OptionsFind.FindFilterColumns <> "*" Then
                If Not _ViewEx.OptionsFind.FindFilterColumns.Split(";"c).Contains(e.Column.FieldName) Then
                    Return
                End If
            End If

            If NeedHighLight(temp) Then
                e.Graphics.FillRectangle(New SolidBrush(_ViewEx.OptionsFind.FindBackGroundColor), e.Bounds)
            End If

            Dim gci As GridCellInfo = TryCast(e.Cell, GridCellInfo)
            Dim tevi As TextEditViewInfo = TryCast(gci.ViewInfo, TextEditViewInfo)
            If tevi Is Nothing Then Return
            Dim textRect As New Rectangle(e.Bounds.X + tevi.MaskBoxRect.X, e.Bounds.Y + tevi.MaskBoxRect.Y, tevi.MaskBoxRect.Width, tevi.MaskBoxRect.Height)

            XPaint.Graphics.DrawMultiColorString(e.Cache, textRect, e.DisplayText, _FilterCellText, e.Appearance, e.Appearance.ForeColor, _ViewEx.OptionsFind.FindHighLightColor, False, filterTextIndex)

            e.Handled = True
        End Sub

        Private Sub ButtonClick(ByVal sender As Object, ByVal e As ButtonPressedEventArgs)
            Dim edit As ButtonEdit = TryCast(sender, ButtonEdit)
            If e.Button Is _ShowAdvancedFilterButton Then
                _ViewEx.OptionsView.ShowXtraPanel = Not _ViewEx.OptionsView.ShowXtraPanel
                If _ViewEx.OptionsView.ShowXtraPanel Then
                    _ShowAdvancedFilterButton.Image = Ressources.filter_delete_16x16
                    _ShowAdvancedFilterButton.ToolTip = "Hide Advanced Filter."

                Else
                    _ShowAdvancedFilterButton.Image = Ressources.filter_16x16
                    _ShowAdvancedFilterButton.ToolTip = "Show Advanced Filter."
                End If

            Else
                Select Case e.Button.Kind
                    Case ButtonPredefines.Search
                        PerformSearch(edit.EditValue)
                    Case ButtonPredefines.Clear
                        ClearEditValue(edit)
                        PerformSearch(Nothing)
                    Case ButtonPredefines.SpinLeft
                        HighLightPrevious()
                    Case ButtonPredefines.SpinRight
                        HighLightNext()
                End Select
            End If
        End Sub

        Private Sub timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
            PerformSearch(_SearchEditor.EditValue)
        End Sub

        Private Sub UpdateShowResult()
            Dim index As Integer
            For index = 0 To _FindList.Keys.Count - 1
                If _FindList(_FindList.Keys.ElementAt(index)) Then
                    index += 1
                    Exit For
                End If
            Next index
            _ShowResultButton.Caption = String.Format("{0} of {1}", index, _FindList.Keys.Count)
        End Sub

        Private Sub HighLightPrevious()

            If FindListIsEmpty Then Return

            Dim currItem As GridCell = _FindList.Keys.ElementAt(0)
            Dim targetItem As GridCell = _FindList.Keys.ElementAt(_FindList.Keys.Count - 1)
            Dim temp As GridCell
            For i As Integer = 1 To _FindList.Keys.Count - 1
                temp = _FindList.Keys.ElementAt(i)
                If _FindList(temp) Then
                    targetItem = _FindList.Keys.ElementAt(i - 1)
                    currItem = temp
                    Exit For
                End If
            Next i

            _FindList(currItem) = False
            _FindList(targetItem) = True
            EnsureCellVisible(targetItem)
            RefreshGridView()
            UpdateShowResult()
        End Sub

        Private Sub HighLightNext()
            If FindListIsEmpty Then
                Return
            End If

            Dim needBreak As Boolean = False
            Dim currItem As GridCell = Nothing
            Dim targetItem As GridCell = Nothing
            For Each item As GridCell In _FindList.Keys
                If needBreak Then
                    targetItem = item
                    Exit For
                End If
                If _FindList(item) Then
                    currItem = item
                    needBreak = True
                End If
            Next item

            If targetItem Is Nothing Then
                targetItem = _FindList.Keys.ElementAt(0)
            End If

            _FindList(currItem) = False
            _FindList(targetItem) = True
            EnsureCellVisible(targetItem)

            RefreshGridView()
            UpdateShowResult()
        End Sub

        Private Sub EnsureCellVisible(ByVal cell As GridCell)
            _ViewEx.MakeRowVisible(cell.RowHandle)
            _ViewEx.MakeColumnVisible(cell.Column)
        End Sub

        Private Sub ClearEditValue(ByVal edit As ButtonEdit)
            edit.EditValue = Nothing
        End Sub

        Private Sub PerformSearch(ByVal val As Object)
            _FindList.Clear()
            _Timer.Stop()
            If val Is Nothing Then
                val = String.Empty
            End If
            _FilterCellText = val.ToString()
            InitFindList()
            If Not FindListIsEmpty Then
                EnsureCellVisible(_FindList.Keys.ElementAt(0))
            End If
            RefreshGridView()
            UpdateShowResult()
        End Sub

        Private Sub InitFindList()
            If String.IsNullOrEmpty(_FilterCellText) Then Return
            For i As Integer = 0 To _ViewEx.RowCount - 1
                For Each col As GridColumn In _ViewEx.Columns
                    If (Not col.Visible) OrElse (TryCast(col.RealColumnEdit, RepositoryItemTextEdit)) Is Nothing Then
                        Continue For
                    End If
                    If _ViewEx.OptionsFind.FindFilterColumns <> "*" Then
                        If Not _ViewEx.OptionsFind.FindFilterColumns.Split(";"c).Contains(col.FieldName) Then
                            Continue For
                        End If
                    End If
                    Dim filterCellText As String = _FilterCellText
                    Dim filterTextIndex As Integer = -1
                    Dim displayText As String = _ViewEx.GetRowCellDisplayText(i, col)
                    If _ViewEx.OptionsFilter.AccentInsensitive Then
                        If Not String.IsNullOrEmpty(displayText) Then
                            displayText = RemoveDiacriticsFunction.Instance.Evaluate(displayText).ToString
                        End If
                        filterCellText = RemoveDiacriticsFunction.Instance.Evaluate(filterCellText).ToString
                    End If
                    filterTextIndex = displayText.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase)

                    If filterTextIndex <> -1 Then
                        _FindList.Add(New GridCell(i, col), False)
                    End If
                Next col
            Next i
            If FindListIsEmpty Then Return
            _FindList(_FindList.Keys.ElementAt(0)) = True
        End Sub

        Private Sub RefreshGridView()
            _ViewEx.LayoutChanged()
        End Sub

        Private Function NeedHighLight(ByVal cell As GridCell) As Boolean
            For Each item As GridCell In _FindList.Keys
                If item.RowHandle = cell.RowHandle AndAlso item.Column Is cell.Column Then
                    Return _FindList(item)
                End If
            Next item
            Return False
        End Function

    End Class
End Namespace


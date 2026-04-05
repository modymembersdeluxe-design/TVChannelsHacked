using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TVChannelsHacked
{
    public sealed class MainForm : Form
    {
        private readonly TextBox _searchBox = new TextBox();
        private readonly ComboBox _countryFilter = new ComboBox();
        private readonly ComboBox _typeFilter = new ComboBox();
        private readonly CheckBox _comingNextHackedOnly = new CheckBox();
        private readonly DataGridView _grid = new DataGridView();
        private readonly CheckedListBox _videoSelector = new CheckedListBox();
        private readonly ListBox _audioSources = new ListBox();
        private readonly ListBox _showCuts = new ListBox();
        private readonly ListBox _comingNext = new ListBox();
        private readonly TextBox _summary = new TextBox();
        private readonly Label _status = new Label();
        private readonly Button _showSelectedButton = new Button();
        private readonly Button _markPlayedButton = new Button();

        private readonly HashSet<string> _playedSources = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly List<ChannelIncident> _all = IncidentRepository.BuildSeedData();

        public MainForm()
        {
            Text = "TV Channels Live Stream & Satellite Incident Browser";
            MinimumSize = new Size(1240, 780);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            BuildLayout();
            BuildFilters();
            WireEvents();
            RefreshGrid();
        }

        private void BuildLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            Controls.Add(root);

            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            filters.Controls.Add(new Label { Text = "Search:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
            _searchBox.Width = 200;
            filters.Controls.Add(_searchBox);

            filters.Controls.Add(new Label { Text = "Country:", AutoSize = true, Padding = new Padding(10, 8, 0, 0) });
            _countryFilter.Width = 160;
            _countryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            filters.Controls.Add(_countryFilter);

            filters.Controls.Add(new Label { Text = "Type:", AutoSize = true, Padding = new Padding(10, 8, 0, 0) });
            _typeFilter.Width = 160;
            _typeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            filters.Controls.Add(_typeFilter);

            _comingNextHackedOnly.Text = "Coming-next hacked only";
            _comingNextHackedOnly.AutoSize = true;
            _comingNextHackedOnly.Padding = new Padding(10, 8, 0, 0);
            filters.Controls.Add(_comingNextHackedOnly);

            root.Controls.Add(filters, 0, 0);
            root.SetColumnSpan(filters, 2);

            _grid.Dock = DockStyle.Fill;
            _grid.AutoGenerateColumns = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.ReadOnly = true;
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Channel", DataPropertyName = nameof(ChannelIncident.ChannelName), Width = 170 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Country", DataPropertyName = nameof(ChannelIncident.Country), Width = 120 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Incident Type", DataPropertyName = nameof(ChannelIncident.IncidentType), Width = 180 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Coming-next Hack", Width = 115, DataPropertyName = nameof(ChannelIncident.IsComingNextHijack) });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Scheduled Block (UTC)", DataPropertyName = nameof(ChannelIncident.ScheduledBlockTimeUtc), Width = 145 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "UTC", DataPropertyName = nameof(ChannelIncident.IncidentTimeUtc), Width = 125 });
            root.Controls.Add(_grid, 0, 1);

            var rightPane = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(8)
            };
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 65f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 18f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 17f));

            var tabs = new TabControl { Dock = DockStyle.Fill };

            var videoTab = new TabPage("Source Videos");
            var videoLayout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            videoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            videoLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38f));
            _videoSelector.Dock = DockStyle.Fill;
            _videoSelector.CheckOnClick = true;
            videoLayout.Controls.Add(_videoSelector, 0, 0);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            _showSelectedButton.Text = "Show Selected Sources";
            _showSelectedButton.AutoSize = true;
            actions.Controls.Add(_showSelectedButton);

            _markPlayedButton.Text = "Use Video Played";
            _markPlayedButton.AutoSize = true;
            actions.Controls.Add(_markPlayedButton);
            videoLayout.Controls.Add(actions, 0, 1);
            videoTab.Controls.Add(videoLayout);

            var audioTab = new TabPage("Music Audio Hacked");
            _audioSources.Dock = DockStyle.Fill;
            audioTab.Controls.Add(_audioSources);

            var cutsTab = new TabPage("Shows Hacked Cuts");
            _showCuts.Dock = DockStyle.Fill;
            cutsTab.Controls.Add(_showCuts);

            tabs.TabPages.Add(videoTab);
            tabs.TabPages.Add(audioTab);
            tabs.TabPages.Add(cutsTab);
            rightPane.Controls.Add(tabs, 0, 0);

            _comingNext.Dock = DockStyle.Fill;
            rightPane.Controls.Add(_comingNext, 0, 1);

            _summary.Multiline = true;
            _summary.ReadOnly = true;
            _summary.Dock = DockStyle.Fill;
            _summary.ScrollBars = ScrollBars.Vertical;
            rightPane.Controls.Add(_summary, 0, 2);

            root.Controls.Add(rightPane, 1, 1);

            _status.Dock = DockStyle.Fill;
            _status.TextAlign = ContentAlignment.MiddleLeft;
            _status.ForeColor = Color.DarkSlateBlue;
            root.Controls.Add(_status, 0, 2);
            root.SetColumnSpan(_status, 2);
        }

        private void BuildFilters()
        {
            _countryFilter.Items.Clear();
            _typeFilter.Items.Clear();

            _countryFilter.Items.Add("All countries");
            _typeFilter.Items.Add("All types");

            foreach (var country in _all.Select(x => x.Country).Distinct().OrderBy(x => x))
            {
                _countryFilter.Items.Add(country);
            }

            foreach (var type in _all.Select(x => x.ContentType).Distinct().OrderBy(x => x))
            {
                _typeFilter.Items.Add(type);
            }

            _countryFilter.SelectedIndex = 0;
            _typeFilter.SelectedIndex = 0;
        }

        private void WireEvents()
        {
            _searchBox.TextChanged += (_, __) => RefreshGrid();
            _countryFilter.SelectedIndexChanged += (_, __) => RefreshGrid();
            _typeFilter.SelectedIndexChanged += (_, __) => RefreshGrid();
            _comingNextHackedOnly.CheckedChanged += (_, __) => RefreshGrid();
            _grid.SelectionChanged += (_, __) => BindSelected();
            _showSelectedButton.Click += (_, __) => ShowSelectedSources();
            _markPlayedButton.Click += (_, __) => MarkSelectedAsPlayed();
        }

        private void RefreshGrid()
        {
            IEnumerable<ChannelIncident> filtered = _all;
            var search = _searchBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(i =>
                    i.ChannelName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    i.Summary.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    i.IncidentType.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (_countryFilter.SelectedIndex > 0 && _countryFilter.SelectedItem is string country)
            {
                filtered = filtered.Where(i => i.Country.Equals(country, StringComparison.OrdinalIgnoreCase));
            }

            if (_typeFilter.SelectedIndex > 0 && _typeFilter.SelectedItem is string type)
            {
                filtered = filtered.Where(i => i.ContentType.Equals(type, StringComparison.OrdinalIgnoreCase));
            }

            if (_comingNextHackedOnly.Checked)
            {
                filtered = filtered.Where(i => i.IsComingNextHijack);
            }

            var list = filtered.OrderByDescending(x => x.IncidentTimeUtc).ToList();
            _grid.DataSource = list;

            var hijackCount = list.Count(x => x.IsComingNextHijack);
            _status.Text = $"Loaded {list.Count} incidents | Coming-next hacked: {hijackCount} | Played videos: {_playedSources.Count} | Windows 8.1-compatible target: .NET Framework 4.8";

            if (list.Count > 0)
            {
                _grid.Rows[0].Selected = true;
                BindSelected();
            }
            else
            {
                _videoSelector.Items.Clear();
                _audioSources.Items.Clear();
                _showCuts.Items.Clear();
                _comingNext.Items.Clear();
                _summary.Text = "No incident matched your filters.";
            }
        }

        private void BindSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is not ChannelIncident incident)
            {
                return;
            }

            _videoSelector.Items.Clear();
            foreach (var video in incident.SourceVideos)
            {
                var isPlayed = _playedSources.Contains(video);
                var label = isPlayed ? $"{video} (played)" : video;
                _videoSelector.Items.Add(label, isPlayed);
            }

            _audioSources.Items.Clear();
            foreach (var audio in incident.SourceAudioTracks)
            {
                _audioSources.Items.Add(audio);
            }

            _showCuts.Items.Clear();
            foreach (var cut in incident.ShowCuts)
            {
                _showCuts.Items.Add(cut);
            }

            _comingNext.Items.Clear();
            _comingNext.Items.Add($"Coming next for {incident.ChannelName}:");
            foreach (var next in incident.ComingNext)
            {
                _comingNext.Items.Add($"• {next}");
            }

            var scheduled = incident.ScheduledBlockTimeUtc.HasValue
                ? incident.ScheduledBlockTimeUtc.Value.ToString("u")
                : "N/A";

            var videoFormats = GetFormats(incident.SourceVideos);
            var audioFormats = GetFormats(incident.SourceAudioTracks);

            _summary.Text =
                $"Channel: {incident.ChannelName}{Environment.NewLine}" +
                $"Country: {incident.Country} / Region: {incident.Region}{Environment.NewLine}" +
                $"Type: {incident.ContentType}{Environment.NewLine}" +
                $"Incident: {incident.IncidentType}{Environment.NewLine}" +
                $"Coming-next hacked: {incident.IsComingNextHijack}{Environment.NewLine}" +
                $"Scheduled block UTC: {scheduled}{Environment.NewLine}" +
                $"Source videos: {incident.SourceVideos.Count} | Formats: {videoFormats}{Environment.NewLine}" +
                $"Audio hacked sources: {incident.SourceAudioTracks.Count} | Formats: {audioFormats}{Environment.NewLine}" +
                $"Show cuts: {incident.ShowCuts.Count}{Environment.NewLine}" +
                $"UTC Time: {incident.IncidentTimeUtc:u}{Environment.NewLine}{Environment.NewLine}" +
                incident.Summary;
        }

        private void MarkSelectedAsPlayed()
        {
            if (_videoSelector.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "Select one or more hacked source videos to mark as played.", "No videos selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var item in _videoSelector.CheckedItems.Cast<object>())
            {
                var text = item.ToString() ?? string.Empty;
                var normalized = text.Replace(" (played)", string.Empty);
                if (!string.IsNullOrWhiteSpace(normalized))
                {
                    _playedSources.Add(normalized);
                }
            }

            BindSelected();
            RefreshGrid();
        }

        private static string GetFormats(IEnumerable<string> files)
        {
            var formats = files
                .Select(x => Path.GetExtension(x).TrimStart('.').ToLowerInvariant())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return formats.Count == 0 ? "n/a" : string.Join(", ", formats);
        }

        private void ShowSelectedSources()
        {
            if (_videoSelector.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "Please choose one or more source videos.", "No videos selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = _videoSelector.CheckedItems.Cast<object>()
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x));
            MessageBox.Show(this, $"Selected source videos:{Environment.NewLine}{string.Join(Environment.NewLine, selected)}", "Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

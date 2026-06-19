// ─── Sala entries ─────────────────────────────────────────────────────────────
let salaEntries = [];

function loadSalaEntries() {
  try { salaEntries = JSON.parse(localStorage.getItem('viaaqua_sala') || '[]'); }
  catch { salaEntries = []; }
}
function saveSalaEntries() {
  localStorage.setItem('viaaqua_sala', JSON.stringify(salaEntries));
}
function addSalaEntry(sede, dia, from, to) {
  salaEntries.push({ id: Date.now(), sede, dia, from, to });
  saveSalaEntries();
}
function removeSalaEntry(id) {
  salaEntries = salaEntries.filter(e => e.id !== id);
  saveSalaEntries();
}

// Returns {from, to, label} for the sala on a given day, or null if closed
function getSalaHorario(sede, dia) {
  if (dia === 'Domingo') {
    const exc = SALA_HORARIOS.excepciones;
    if (!exc.sedes.includes(sede)) return null;
    return { from: '09:00', to: '13:00', label: '09:00 – 13:00' };
  }
  if (dia === 'Sábado') return { from: '09:00', to: '18:00', label: '09:00 – 18:00' };
  return { from: '06:30', to: '22:00', label: '06:30 – 22:00' };
}

// ─── State ────────────────────────────────────────────────────────────────────
const state = {
  sedes:       [],
  dias:        [],
  actividades: [],
  timeFrom:    '07:00',
  timeTo:      '22:00',
  search:      '',
  view:        'cards',
  favorites:   new Set(),
};

// ─── Time helpers ─────────────────────────────────────────────────────────────
const SLIDER_MIN = 420;   // 07:00 in minutes
const SLIDER_MAX = 1320;  // 22:00 in minutes

function minsToTime(mins) {
  const h = Math.floor(mins / 60);
  const m = mins % 60;
  return `${String(h).padStart(2,'0')}:${String(m).padStart(2,'0')}`;
}

function timeToMins(hhmm) {
  const [h, m] = hhmm.split(':').map(Number);
  return h * 60 + m;
}

function updateSliderUI() {
  const fromMins = timeToMins(state.timeFrom);
  const toMins   = timeToMins(state.timeTo);
  const range    = SLIDER_MAX - SLIDER_MIN;

  const pctFrom = ((fromMins - SLIDER_MIN) / range) * 100;
  const pctTo   = ((toMins   - SLIDER_MIN) / range) * 100;

  document.getElementById('slider-range').style.left  = pctFrom + '%';
  document.getElementById('slider-range').style.width = (pctTo - pctFrom) + '%';
  document.getElementById('label-from').textContent   = state.timeFrom;
  document.getElementById('label-to').textContent     = state.timeTo;
  document.getElementById('slider-from').value = fromMins;
  document.getElementById('slider-to').value   = toMins;
}

// ─── Helpers ──────────────────────────────────────────────────────────────────
function getDayLabel(dia) {
  return { LMV: 'Lun · Mié · Vie', MJ: 'Mar · Jue', Sabado: 'Sábado' }[dia] || dia;
}

function isAvailable(hora) {
  return hora >= '14:00' && hora < '18:00';
}

// ─── Favorites ────────────────────────────────────────────────────────────────
// state.favorites = { [classId]: ['Lunes', 'Miércoles', ...] }
const GROUP_DAYS = {
  'LMV':    ['Lunes', 'Miércoles', 'Viernes'],
  'MJ':     ['Martes', 'Jueves'],
  'Sabado': ['Sábado'],
};

function loadFavorites() {
  try {
    const saved = JSON.parse(localStorage.getItem('viaaqua_favorites2') || 'null');
    if (saved && typeof saved === 'object' && !Array.isArray(saved)) {
      state.favorites = saved;
    } else {
      // Migrate old Set-style favorites: default to all days in group
      const old = JSON.parse(localStorage.getItem('viaaqua_favorites') || '[]');
      state.favorites = {};
      old.forEach(id => {
        const c = CLASES.find(x => x.id === id);
        if (c) state.favorites[id] = [...(GROUP_DAYS[c.dia] || [])];
      });
    }
  } catch { state.favorites = {}; }
}

function saveFavorites() {
  localStorage.setItem('viaaqua_favorites2', JSON.stringify(state.favorites));
}

function isFaved(id) {
  return state.favorites[id]?.length > 0;
}

function getFavDays(id) {
  return state.favorites[id] || [];
}

function setFavDays(id, days) {
  if (!days || days.length === 0) {
    delete state.favorites[id];
  } else {
    state.favorites[id] = days;
  }
  saveFavorites();
}

// Show day-picker popup near a button element
function showDayPicker(btn, clase, onDone) {
  document.querySelectorAll('.day-picker-popup').forEach(el => el.remove());

  const available = GROUP_DAYS[clase.dia] || [];
  const current   = getFavDays(clase.id);

  const popup = document.createElement('div');
  popup.className = 'day-picker-popup';
  popup.innerHTML = `
    <div class="dp-title">¿Qué días?</div>
    <div class="dp-days">
      ${available.map(d => `
        <label class="dp-day">
          <input type="checkbox" value="${d}" ${current.includes(d) ? 'checked' : ''}>
          <span>${d}</span>
        </label>`).join('')}
    </div>
    <div class="dp-actions">
      <button class="dp-btn-cancel">Quitar</button>
      <button class="dp-btn-ok">Listo</button>
    </div>`;

  // Position near the button
  document.body.appendChild(popup);
  const rect = btn.getBoundingClientRect();
  const pw = popup.offsetWidth || 160;
  let left = rect.left + window.scrollX - pw / 2 + rect.width / 2;
  left = Math.max(8, Math.min(left, window.innerWidth - pw - 8));
  popup.style.left = left + 'px';
  popup.style.top  = (rect.bottom + window.scrollY + 6) + 'px';

  const close = () => popup.remove();

  popup.querySelector('.dp-btn-ok').addEventListener('click', () => {
    const selected = [...popup.querySelectorAll('input:checked')].map(i => i.value);
    setFavDays(clase.id, selected);
    close();
    onDone();
  });

  popup.querySelector('.dp-btn-cancel').addEventListener('click', () => {
    setFavDays(clase.id, []);
    close();
    onDone();
  });

  // Close on outside click
  setTimeout(() => {
    document.addEventListener('click', function outside(e) {
      if (!popup.contains(e.target) && e.target !== btn) {
        close();
        document.removeEventListener('click', outside);
      }
    });
  }, 0);
}

// ─── URL params ───────────────────────────────────────────────────────────────
function parseURL() {
  const p = new URLSearchParams(window.location.search);
  if (p.get('sede'))      state.sedes       = p.get('sede').split(',').filter(Boolean);
  if (p.get('dia'))       state.dias        = p.get('dia').split(',').filter(Boolean);
  if (p.get('actividad')) state.actividades = p.get('actividad').split(',').filter(Boolean);
  if (p.get('desde'))     state.timeFrom    = p.get('desde');
  if (p.get('hasta'))     state.timeTo      = p.get('hasta');
  if (p.get('q'))         state.search      = p.get('q');
  if (p.get('view'))      state.view        = p.get('view');
}

function buildShareURL() {
  const p = new URLSearchParams();
  if (state.sedes.length)                       p.set('sede',      state.sedes.join(','));
  if (state.dias.length)                        p.set('dia',       state.dias.join(','));
  if (state.actividades.length)                 p.set('actividad', state.actividades.join(','));
  if (state.timeFrom !== '07:00')               p.set('desde',     state.timeFrom);
  if (state.timeTo   !== '22:00')               p.set('hasta',     state.timeTo);
  if (state.search)                             p.set('q',         state.search);
  if (state.view !== 'cards')                   p.set('view',      state.view);
  const qs = p.toString();
  return `${location.origin}${location.pathname}${qs ? '?' + qs : ''}`;
}

// ─── Filtering ────────────────────────────────────────────────────────────────
function getFilteredClasses() {
  const q = state.search.toLowerCase();
  const selectedGroups = new Set(state.dias.map(d => DAY_TO_GROUP[d]));

  return CLASES.filter(c => {
    if (state.sedes.length       && !state.sedes.includes(c.sede))            return false;
    if (state.dias.length        && !selectedGroups.has(c.dia))               return false;
    if (c.hora < state.timeFrom  || c.hora > state.timeTo)                    return false;
    if (state.actividades.length && !state.actividades.includes(c.actividad)) return false;
    if (q && !c.actividad.toLowerCase().includes(q))                          return false;
    return true;
  }).sort((a, b) => a.hora.localeCompare(b.hora) || a.actividad.localeCompare(b.actividad));
}

// ─── Simultaneous detection ───────────────────────────────────────────────────
function buildSimMap(classes) {
  const byHora = {};
  classes.forEach(c => (byHora[c.hora] = byHora[c.hora] || []).push(c.id));
  const simMap = {};
  Object.values(byHora).forEach(ids => {
    if (ids.length >= 2) ids.forEach(id => (simMap[id] = ids.length));
  });
  return simMap;
}

// ─── Card builder ─────────────────────────────────────────────────────────────
function buildCard(c, simMap) {
  const avail     = isAvailable(c.hora);
  const simCount  = simMap[c.id];
  const fav       = isFaved(c.id);
  const favDays   = getFavDays(c.id);
  const sedeColor = SEDES[c.sede].color;
  const singleDay = GROUP_DAYS[c.dia]?.length === 1;

  const favDaysLabel = fav && !singleDay
    ? `<span class="fav-days-label">${favDays.map(d => d.slice(0,3)).join(' · ')}</span>`
    : '';

  return `
    <article class="class-card${avail ? ' available' : ''}" data-id="${c.id}">
      <div class="card-sede-bar" style="background:${sedeColor}"></div>
      <div class="card-body">
        <div class="card-top">
          <span class="card-activity">${escHtml(c.actividad)}</span>
          <div class="card-fav-wrap">
            ${favDaysLabel}
            <button class="btn-fav${fav ? ' active' : ''}" data-id="${c.id}"
              title="${fav ? 'Editar días en mi rutina' : 'Agregar a mi rutina'}"
              aria-pressed="${fav}">★</button>
          </div>
        </div>
        <div class="card-meta">
          <span class="badge-sede" style="--color:${sedeColor}">${escHtml(c.sede)}</span>
          <span class="card-day">${getDayLabel(c.dia)}</span>
          <span class="card-time">${c.hora}</span>
        </div>
        ${avail || simCount ? `<div class="card-badges">
          ${avail    ? '<span class="badge badge-available">Tu franja</span>' : ''}
          ${simCount ? `<span class="badge badge-sim">${simCount} simultáneas</span>` : ''}
        </div>` : ''}
      </div>
    </article>`;
}

function escHtml(s) {
  return s.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
}

function bindCardFavs(root) {
  root.querySelectorAll('.btn-fav').forEach(btn => {
    btn.addEventListener('click', e => {
      e.stopPropagation();
      const id    = Number(btn.dataset.id);
      const clase = CLASES.find(c => c.id === id);
      if (!clase) return;

      const singleDay = GROUP_DAYS[clase.dia]?.length === 1;
      if (singleDay) {
        // Sábado: just toggle
        if (isFaved(id)) setFavDays(id, []);
        else setFavDays(id, [...GROUP_DAYS[clase.dia]]);
        updateFavBtn(btn, id);
        if (state.view === 'rutina') renderRutina();
      } else {
        // LMV / MJ: show day picker
        showDayPicker(btn, clase, () => {
          updateFavBtn(btn, id);
          if (state.view === 'rutina') renderRutina();
        });
      }
    });
  });
}

function updateFavBtn(btn, id) {
  const fav = isFaved(id);
  btn.classList.toggle('active', fav);
  btn.setAttribute('aria-pressed', fav);
  btn.title = fav ? 'Editar días en mi rutina' : 'Agregar a mi rutina';
  // Update fav-days-label if present
  const wrap = btn.closest('.card-fav-wrap');
  if (wrap) {
    const label = wrap.querySelector('.fav-days-label');
    const days  = getFavDays(id);
    if (fav && days.length > 0) {
      const text = days.map(d => d.slice(0,3)).join(' · ');
      if (label) label.textContent = text;
      else {
        const span = document.createElement('span');
        span.className = 'fav-days-label';
        span.textContent = text;
        wrap.insertBefore(span, btn);
      }
    } else if (label) label.remove();
  }
}

// ─── Render: Cards ────────────────────────────────────────────────────────────
function renderCards(classes) {
  const simMap = buildSimMap(classes);
  const grid   = document.getElementById('cards-grid');

  if (!classes.length) {
    grid.innerHTML = `<div class="empty-state">
      <span class="empty-icon">🔍</span>
      <span>No hay clases con los filtros seleccionados.</span>
    </div>`;
    return;
  }
  grid.innerHTML = classes.map(c => buildCard(c, simMap)).join('');
  bindCardFavs(grid);
}

// ─── Render: Grid ─────────────────────────────────────────────────────────────
function renderGrid(classes) {
  const fromMins = timeToMins(state.timeFrom);
  const toMins   = timeToMins(state.timeTo);
  const PPM      = 2;                              // 2px per minute = 120px per hour
  const TOTAL    = (toMins - fromMins) * PPM;

  const simMap = buildSimMap(classes);

  // Individual day columns — filter to selected days if any
  const ALL_COLS = [
    { key: 'Lunes',     group: 'LMV',    short: 'Lun' },
    { key: 'Martes',    group: 'MJ',     short: 'Mar' },
    { key: 'Miércoles', group: 'LMV',    short: 'Mié' },
    { key: 'Jueves',    group: 'MJ',     short: 'Jue' },
    { key: 'Viernes',   group: 'LMV',    short: 'Vie' },
    { key: 'Sábado',    group: 'Sabado', short: 'Sáb' },
  ];
  const cols = state.dias.length
    ? ALL_COLS.filter(c => state.dias.includes(c.key))
    : ALL_COLS;

  // Group filtered classes by dia group
  const byGroup = { LMV: [], MJ: [], Sabado: [] };
  classes.forEach(c => { if (byGroup[c.dia]) byGroup[c.dia].push(c); });

  // Time axis — label every hour, half-line every 30 min
  let timeMarkers = '', hourLines = '';
  for (let m = fromMins; m <= toMins; m += 30) {
    const top    = (m - fromMins) * PPM;
    const isHour = m % 60 === 0;
    hourLines += `<div class="hour-line${isHour ? '' : ' half'}" style="top:${top}px"></div>`;
    if (isHour)
      timeMarkers += `<div class="time-marker" style="top:${top}px">${minsToTime(m)}</div>`;
  }

  // Available band 14:00–18:00
  const availFrom = Math.max(14 * 60, fromMins);
  const availTo   = Math.min(18 * 60, toMins);
  const availBand = availFrom < availTo
    ? `<div class="available-band"
         style="top:${(availFrom-fromMins)*PPM}px;height:${(availTo-availFrom)*PPM}px"
         title="Tu franja de disponibilidad (14:00–18:00)"></div>`
    : '';

  // Build day columns
  const dayColumnsHtml = cols.map(col => {
    const colClasses = byGroup[col.group] || [];

    // Group by hora for side-by-side conflict rendering
    const byHora = {};
    colClasses.forEach(c => (byHora[c.hora] = byHora[c.hora] || []).push(c));

    const cards = colClasses.map(c => {
      const cMins    = timeToMins(c.hora);
      const top      = (cMins - fromMins) * PPM;
      const siblings = byHora[c.hora];
      const idx      = siblings.indexOf(c);
      const total    = siblings.length;
      const gap      = total > 1 ? 0.5 : 0;
      const w        = (100 / total) - gap;
      const left     = idx * (100 / total);
      const avail    = isAvailable(c.hora);
      const fav      = isFaved(c.id);
      const simCount = simMap[c.id];

      return `<div class="grid-card${avail ? ' available' : ''}"
        style="top:${top}px;left:${left}%;width:${w}%;--sede-color:${SEDES[c.sede].color}"
        data-id="${c.id}"
        title="${escHtml(c.actividad)} · ${escHtml(c.sede)} · ${c.hora}">
        <span class="gc-sede-dot" style="background:${SEDES[c.sede].color}"></span>
        <span class="gc-name">${escHtml(c.actividad)}</span>
        <span class="gc-time">${c.hora}</span>
        ${simCount ? `<span class="gc-sim">${simCount}</span>` : ''}
        <button class="gc-fav${fav ? ' active' : ''}" data-id="${c.id}" aria-pressed="${fav}">★</button>
      </div>`;
    }).join('');

    return `<div class="day-column" style="height:${TOTAL}px">
      ${hourLines}${cards}
    </div>`;
  }).join('');

  document.getElementById('view-grid').innerHTML = `
    <div class="grid-view-wrap">
      <div class="grid-view-inner">
        <div class="grid-header-row">
          <div class="grid-time-spacer"></div>
          ${cols.map(c => `<div class="grid-day-header"><span class="ghd-full">${c.key}</span><span class="ghd-short">${c.short}</span></div>`).join('')}
        </div>
        <div class="grid-body-row">
          <div class="grid-time-col" style="height:${TOTAL}px">${timeMarkers}</div>
          <div class="grid-days-wrap" style="height:${TOTAL}px">
            ${availBand}
            ${dayColumnsHtml}
          </div>
        </div>
      </div>
    </div>`;

  document.getElementById('view-grid').querySelectorAll('.gc-fav').forEach(btn => {
    btn.addEventListener('click', e => {
      e.stopPropagation();
      const id    = Number(btn.dataset.id);
      const clase = CLASES.find(c => c.id === id);
      if (!clase) return;
      const singleDay = GROUP_DAYS[clase.dia]?.length === 1;
      if (singleDay) {
        if (isFaved(id)) setFavDays(id, []);
        else setFavDays(id, [...GROUP_DAYS[clase.dia]]);
        btn.classList.toggle('active', isFaved(id));
        btn.setAttribute('aria-pressed', isFaved(id));
      } else {
        showDayPicker(btn, clase, () => {
          btn.classList.toggle('active', isFaved(id));
          btn.setAttribute('aria-pressed', isFaved(id));
        });
      }
    });
  });
}

// ─── Render: Rutina ───────────────────────────────────────────────────────────
function buildSalaCard(entry) {
  const sedeColor = SEDES[entry.sede]?.color || '#666';
  return `
    <article class="class-card sala-libre-card">
      <div class="card-sede-bar" style="background:${sedeColor}"></div>
      <div class="card-body">
        <div class="card-top">
          <span class="card-activity">🏋 Sala libre</span>
          <button class="btn-remove-sala" data-id="${entry.id}" title="Quitar de mi rutina">✕</button>
        </div>
        <div class="card-meta">
          <span class="badge-sede" style="--color:${sedeColor}">${escHtml(entry.sede)}</span>
          <span class="card-day">${escHtml(entry.dia)}</span>
          <span class="card-time">${entry.from} – ${entry.to}</span>
        </div>
      </div>
    </article>`;
}

function renderSalaForm() {
  const sedeOptions = SEDE_NAMES.map(s =>
    `<option value="${escHtml(s)}">${escHtml(s)}</option>`).join('');
  const allDias = [...INDIVIDUAL_DAYS, 'Domingo'];
  const diaOptions = allDias.map(d =>
    `<option value="${escHtml(d)}">${escHtml(d)}</option>`).join('');

  return `
    <div class="sala-add-form" id="sala-add-form">
      <span class="sala-form-label">+ Sala libre</span>
      <select class="sala-select" id="sf-sede"><option value="">Sede…</option>${sedeOptions}</select>
      <select class="sala-select" id="sf-dia"><option value="">Día…</option>${diaOptions}</select>
      <input type="time" class="sala-time-in" id="sf-from" value="07:00">
      <span class="sala-dash">–</span>
      <input type="time" class="sala-time-in" id="sf-to" value="08:00">
      <button class="btn-sala-add" id="btn-sala-confirm">Agregar</button>
    </div>`;
}

function renderRutina() {
  const container = document.getElementById('view-rutina');

  const favs = CLASES.filter(c => isFaved(c.id))
    .sort((a, b) => a.hora.localeCompare(b.hora) || a.actividad.localeCompare(b.actividad));

  // Group classes and sala by individual day (using user-selected days)
  const DAY_ORDER = ['Lunes','Martes','Miércoles','Jueves','Viernes','Sábado','Domingo'];
  const byDay = {};
  DAY_ORDER.forEach(d => (byDay[d] = { clases: [], sala: [] }));

  favs.forEach(c => {
    const selectedDays = getFavDays(c.id);
    selectedDays.forEach(d => { if (byDay[d]) byDay[d].clases.push(c); });
  });
  salaEntries.forEach(e => {
    if (byDay[e.dia]) byDay[e.dia].sala.push(e);
  });

  const hasContent = favs.length || salaEntries.length;

  const simMap = buildSimMap(favs);
  const sectionsHtml = DAY_ORDER
    .filter(d => byDay[d].clases.length || byDay[d].sala.length)
    .map(d => {
      const clasesHtml = byDay[d].clases.map(c => buildCard(c, simMap)).join('');
      const salaHtml   = byDay[d].sala.map(e => buildSalaCard(e)).join('');
      return `
        <section class="rutina-day-section">
          <h3 class="rutina-day-label">${d}</h3>
          <div class="cards-grid">${clasesHtml}${salaHtml}</div>
        </section>`;
    }).join('');

  const shareBtn = hasContent
    ? `<button class="btn-share-rutina" id="btn-share-rutina">⬆ Compartir mi rutina</button>`
    : '';

  container.innerHTML = `
    <div class="rutina-wrap">
      <div class="rutina-top-bar">
        ${renderSalaForm()}
        ${shareBtn}
      </div>
      ${hasContent ? sectionsHtml : `<div class="empty-state">
        <span class="empty-icon">★</span>
        <span>Tu rutina está vacía.</span>
        <span>Marcá clases con ★ o agregá un horario de sala arriba.</span>
      </div>`}
    </div>`;

  bindCardFavs(container);

  // Remove sala entry buttons
  container.querySelectorAll('.btn-remove-sala').forEach(btn => {
    btn.addEventListener('click', () => {
      removeSalaEntry(Number(btn.dataset.id));
      renderRutina();
    });
  });

  // Sala form submit
  document.getElementById('btn-sala-confirm').addEventListener('click', () => {
    const sede = document.getElementById('sf-sede').value;
    const dia  = document.getElementById('sf-dia').value;
    const from = document.getElementById('sf-from').value;
    const to   = document.getElementById('sf-to').value;

    if (!sede || !dia) { showToast('Elegí sede y día'); return; }
    if (!from || !to || from >= to) { showToast('Horario inválido'); return; }

    // Validate against sala schedule
    const horario = getSalaHorario(sede, dia);
    if (!horario) {
      showToast(`${sede} no abre los ${dia}s`);
      return;
    }
    if (from < horario.from || to > horario.to) {
      showToast(`Sala abierta ${horario.label}`);
      return;
    }

    addSalaEntry(sede, dia, from, to);
    renderRutina();
    showToast('Sala agregada a tu rutina ✓');
  });

  // Share rutina button
  document.getElementById('btn-share-rutina')?.addEventListener('click', () => {
    const text = buildRutinaText(byDay, DAY_ORDER);
    navigator.clipboard.writeText(text).then(() => showToast('¡Rutina copiada al portapapeles!'));
  });
}

function buildRutinaText(byDay, DAY_ORDER) {
  const lines = ['🏋 MI RUTINA — VIAAQUA', ''];
  DAY_ORDER.forEach(dia => {
    const { clases, sala } = byDay[dia];
    if (!clases.length && !sala.length) return;
    lines.push(`📅 ${dia.toUpperCase()}`);
    const items = [
      ...clases.map(c => `  ${c.hora}  ${c.actividad} · ${c.sede}`),
      ...sala.map(e => `  ${e.from}–${e.to}  Sala libre · ${e.sede}`),
    ].sort();
    items.forEach(l => lines.push(l));
    lines.push('');
  });
  return lines.join('\n').trimEnd();
}

// ─── Render: Activities filter ────────────────────────────────────────────────
function renderActividadesFilter() {
  const activities = [...new Set(CLASES.map(c => c.actividad))].sort((a, b) => a.localeCompare(b));
  const container  = document.getElementById('filter-actividad');

  container.innerHTML = activities.map(act => `
    <label class="chip act-chip">
      <input type="checkbox" value="${escHtml(act)}"${state.actividades.includes(act) ? ' checked' : ''}>
      <span>${escHtml(act)}</span>
    </label>`).join('');

  container.querySelectorAll('input').forEach(inp => {
    inp.addEventListener('change', () => {
      state.actividades = [...container.querySelectorAll('input:checked')].map(i => i.value);
      render();
    });
  });
}

// ─── Sync filter UI ───────────────────────────────────────────────────────────
function syncFilterUI() {
  document.querySelectorAll('#filter-sede input').forEach(inp => {
    inp.checked = state.sedes.includes(inp.value);
  });
  document.querySelectorAll('#filter-dia input').forEach(inp => {
    inp.checked = state.dias.includes(inp.value);
  });
  document.getElementById('filter-search').value = state.search;
  updateSliderUI();
}

// ─── Main render ──────────────────────────────────────────────────────────────
function render() {
  document.querySelectorAll('.view').forEach(v => v.classList.remove('active'));
  document.getElementById('view-' + state.view).classList.add('active');

  document.querySelectorAll('.view-tab').forEach(btn => {
    const isActive = btn.dataset.view === state.view;
    btn.classList.toggle('active', isActive);
    btn.setAttribute('aria-selected', isActive);
  });

  if (state.view === 'rutina') {
    document.getElementById('results-count').textContent =
      `${Object.keys(state.favorites).length} clase${Object.keys(state.favorites).length !== 1 ? 's' : ''} en tu rutina`;
    document.getElementById('sim-warning').style.display = 'none';
    renderRutina();
    return;
  }

  const filtered = getFilteredClasses();
  document.getElementById('results-count').textContent =
    `${filtered.length} clase${filtered.length !== 1 ? 's' : ''}`;

  const simMap = buildSimMap(filtered);
  document.getElementById('sim-warning').style.display =
    Object.keys(simMap).length > 0 ? '' : 'none';

  if (state.view === 'cards') renderCards(filtered);
  if (state.view === 'grid')  renderGrid(filtered);
}

// ─── Event wiring ─────────────────────────────────────────────────────────────
function bindEvents() {
  // Sede
  document.querySelectorAll('#filter-sede input').forEach(inp => {
    inp.addEventListener('change', () => {
      state.sedes = [...document.querySelectorAll('#filter-sede input:checked')].map(i => i.value);
      render();
    });
  });

  // Día
  document.querySelectorAll('#filter-dia input').forEach(inp => {
    inp.addEventListener('change', () => {
      state.dias = [...document.querySelectorAll('#filter-dia input:checked')].map(i => i.value);
      render();
    });
  });

  // Time range slider
  const sliderFrom = document.getElementById('slider-from');
  const sliderTo   = document.getElementById('slider-to');

  sliderFrom.addEventListener('input', () => {
    let val = parseInt(sliderFrom.value);
    const toVal = parseInt(sliderTo.value);
    if (val >= toVal - 15) { val = toVal - 15; sliderFrom.value = val; }
    state.timeFrom = minsToTime(val);
    updateSliderUI();
    render();
  });

  sliderTo.addEventListener('input', () => {
    let val = parseInt(sliderTo.value);
    const fromVal = parseInt(sliderFrom.value);
    if (val <= fromVal + 15) { val = fromVal + 15; sliderTo.value = val; }
    state.timeTo = minsToTime(val);
    updateSliderUI();
    render();
  });

  // Search
  document.getElementById('filter-search').addEventListener('input', e => {
    state.search = e.target.value.trim();
    render();
  });

  // View tabs
  document.querySelectorAll('.view-tab').forEach(btn => {
    btn.addEventListener('click', () => {
      state.view = btn.dataset.view;
      render();
      closeFilters();
    });
  });

  // Clear filters
  document.getElementById('btn-clear-filters').addEventListener('click', () => {
    state.sedes       = [];
    state.dias        = [];
    state.actividades = [];
    state.timeFrom    = '07:00';
    state.timeTo      = '22:00';
    state.search      = '';
    syncFilterUI();
    renderActividadesFilter();
    render();
  });

  // Share
  document.getElementById('btn-share').addEventListener('click', () => {
    navigator.clipboard.writeText(buildShareURL())
      .then(() => showToast('URL copiada al portapapeles ✓'))
      .catch(() => showToast('No se pudo copiar la URL'));
  });

  // Mobile filter toggle
  document.getElementById('btn-filter-toggle').addEventListener('click', openFilters);
  document.getElementById('filter-overlay').addEventListener('click', closeFilters);
}

function openFilters() {
  document.getElementById('filters-panel').classList.add('open');
  document.getElementById('filter-overlay').classList.add('active');
}
function closeFilters() {
  document.getElementById('filters-panel').classList.remove('open');
  document.getElementById('filter-overlay').classList.remove('active');
}

// ─── Toast ────────────────────────────────────────────────────────────────────
let _toastTimer;
function showToast(msg) {
  const t = document.getElementById('toast');
  t.textContent = msg;
  t.classList.add('show');
  clearTimeout(_toastTimer);
  _toastTimer = setTimeout(() => t.classList.remove('show'), 2800);
}

// ─── Init ─────────────────────────────────────────────────────────────────────
function init() {
  loadFavorites();
  parseURL();
  renderActividadesFilter();
  syncFilterUI();
  bindEvents();
  render();
}

document.addEventListener('DOMContentLoaded', init);

// ─── Sedes ────────────────────────────────────────────────────────────────────
const SEDES = {
  'Buceo':          { color: '#378ADD', label: 'Buceo' },
  'Malvín':         { color: '#1D9E75', label: 'Malvín' },
  'Punta Carretas': { color: '#D4537E', label: 'Punta Carretas' },
  'Pocitos':        { color: '#BA7517', label: 'Pocitos' },
  'Punta del Este': { color: '#9333EA', label: 'Punta del Este' },
};

const SEDE_NAMES = Object.keys(SEDES);

// ─── Días ─────────────────────────────────────────────────────────────────────
const DIAS = [
  { value: 'LMV',    label: 'Lun · Mié · Vie' },
  { value: 'MJ',     label: 'Mar · Jue' },
  { value: 'Sabado', label: 'Sábado' },
];

// ─── Franjas ─────────────────────────────────────────────────────────────────
const FRANJAS = [
  { value: 'manana',   label: 'Mañana',   sublabel: '07:00–12:00', from: '07:00', to: '12:00' },
  { value: 'mediodia', label: 'Mediodía', sublabel: '12:00–15:00', from: '12:00', to: '15:00' },
  { value: 'tarde',    label: 'Tarde',    sublabel: '15:00–18:00', from: '15:00', to: '18:00' },
  { value: 'noche',    label: 'Noche',    sublabel: '18:00–22:00', from: '18:00', to: '22:00' },
];

// ─── Raw class data ───────────────────────────────────────────────────────────
const RAW = {
  'Buceo': {
    'LMV': [
      ['07:10', 'Entrenamiento Funcional'],
      ['07:10', 'Pilates Reformer'],
      ['08:00', 'Ciclismo'],
      ['08:00', 'Hidrotraining'],
      ['08:00', 'Pilates Reformer'],
      ['08:30', 'Entrenamiento Funcional'],
      ['09:00', 'Gimnasia para el Bienestar'],
      ['09:00', 'Pilates Reformer'],
      ['09:00', 'Pilates Circuit'],
      ['09:50', 'Pilates Reformer'],
      ['10:00', 'Yoga'],
      ['10:00', 'Super Local'],
      ['10:15', 'Hidrotraining'],
      ['10:40', 'Pilates Reformer'],
      ['11:10', 'Postura y Movilidad'],
      ['11:30', 'Pilates Reformer'],
      ['11:30', 'Hidrotraining'],
      ['12:00', 'Entrenamiento Funcional'],
      ['13:30', 'Ciclismo'],
      ['14:00', 'Pilates Reformer'],
      ['15:00', 'Pilates Reformer'],
      ['15:00', 'Hidrotraining'],
      ['16:00', 'Aerolocal'],
      ['16:00', 'Pilates Reformer'],
      ['16:00', 'Hidrotraining'],
      ['17:00', 'Hidrotraining'],
      ['17:00', 'Pilates Reformer'],
      ['17:30', 'Pilates Mat'],
      ['18:00', 'Ciclismo'],
      ['18:00', 'Pilates Reformer'],
      ['18:00', 'Burning HIIT'],
      ['18:00', 'Ritmos'],
      ['18:40', 'Pilates Circuit'],
      ['18:50', 'Entrenamiento Funcional'],
      ['19:00', 'Pilates Reformer'],
      ['19:10', 'Super Local'],
      ['19:15', 'Ciclismo'],
      ['19:15', 'Hidrotraining'],
      ['20:00', 'Cross Training'],
      ['20:00', 'Pilates Reformer'],
      ['20:20', 'Ciclismo Local'],
    ],
    'MJ': [
      ['07:10', 'Pilates Reformer'],
      ['08:00', 'Pilates Reformer'],
      ['08:00', 'Hidrotraining'],
      ['08:05', 'Entrenamiento Funcional'],
      ['09:00', 'Pilates Reformer'],
      ['09:00', 'Pilates Circuit'],
      ['09:10', 'Super Local'],
      ['09:15', 'Hidrotraining'],
      ['09:30', 'Taichi'],
      ['09:50', 'Pilates Reformer'],
      ['10:10', 'Pilates Mat'],
      ['10:15', 'Ciclismo Local'],
      ['10:30', 'Hidropilates'],
      ['10:40', 'Pilates Reformer'],
      ['11:30', 'Pilates Reformer'],
      ['15:00', 'Hidrotraining'],
      ['15:00', 'Pilates Reformer'],
      ['16:00', 'Hidrotraining'],
      ['16:00', 'Pilates Reformer'],
      ['17:00', 'Pilates Reformer'],
      ['17:00', 'Hidrotraining'],
      ['17:00', 'Postura y Movilidad'],
      ['18:00', 'Pilates Reformer'],
      ['18:00', 'Entrenamiento Funcional'],
      ['18:10', 'Pilates Mat'],
      ['18:10', 'Aerolocal'],
      ['18:30', 'Ciclismo'],
      ['19:00', 'Pilates Reformer'],
      ['19:00', 'Entrenamiento Funcional'],
      ['19:10', 'Yoga'],
      ['19:10', 'Super Local'],
      ['19:15', 'Hidrotraining'],
      ['19:40', 'Ciclismo'],
      ['20:00', 'Pilates Reformer'],
      ['20:00', 'Burning HIIT'],
      ['20:20', 'Power Yoga'],
    ],
    'Sabado': [
      ['09:15', 'Reformer Exclusivo Principiantes'],
      ['10:00', 'Taichi'],
      ['10:10', 'Pilates Reformer'],
      ['10:15', 'Hidrotraining'],
      ['11:00', 'Entrenamiento Funcional'],
      ['11:10', 'Pilates Reformer'],
      ['11:15', 'Hidrotraining'],
      ['12:10', 'Pilates Reformer'],
    ],
  },
  'Malvín': {
    'LMV': [
      ['07:15', 'Entrenamiento Funcional'],
      ['08:00', 'Super Local'],
      ['08:30', 'Ciclismo'],
      ['09:00', 'Pilates Circuit'],
      ['10:00', 'Postura y Movilidad'],
      ['16:00', 'Super Local'],
      ['17:00', 'Pilates Mat'],
      ['18:00', 'Yoga'],
      ['18:15', 'Entrenamiento Funcional'],
      ['18:30', 'Ciclismo'],
      ['19:00', 'Aerolocal'],
      ['19:15', 'Entrenamiento Funcional'],
      ['19:30', 'Ciclismo'],
      ['20:00', 'Pilates Circuit'],
      ['20:15', 'ABS & HIIT'],
    ],
    'MJ': [
      ['07:15', 'Ciclismo'],
      ['08:00', 'Aerolocal'],
      ['08:15', 'Entrenamiento Funcional'],
      ['09:00', 'Pilates Mat'],
      ['10:00', 'Postura y Movilidad'],
      ['17:00', 'Pilates Circuit'],
      ['18:00', 'Fit Pilates'],
      ['18:15', 'Entrenamiento Funcional'],
      ['18:30', 'Ciclismo'],
      ['19:00', 'Full Body'],
      ['19:15', 'Cross Training'],
      ['19:30', 'Ciclismo'],
      ['20:00', 'Pilates Circuit'],
    ],
    'Sabado': [
      ['10:00', 'Super Local'],
      ['11:15', 'Entrenamiento Funcional'],
    ],
  },
};

// Punta Carretas = Buceo · Pocitos = Malvín
RAW['Punta Carretas'] = RAW['Buceo'];
RAW['Pocitos']        = RAW['Malvín'];

RAW['Punta del Este'] = {
  'LMV': [
    ['07:00', 'Pilates Reformer'],
    ['07:30', 'Entrenamiento Funcional'],
    ['08:00', 'Aerolocal'],
    ['08:00', 'Pilates Reformer'],
    ['08:30', 'Ciclismo'],
    ['09:00', 'Pilates Reformer'],
    ['09:00', 'Postura y Movilidad'],
    ['09:30', 'Hidrotraining'],
    ['10:00', 'Pilates Reformer'],
    ['10:30', 'Hidrotraining'],
    ['11:30', 'Gimnasia para el Bienestar'],
    ['11:30', 'Entrenamiento Funcional'],
    ['14:00', 'Pilates Reformer'],
    ['15:00', 'Pilates Reformer'],
    ['16:00', 'Pilates Reformer'],
    ['17:00', 'Yoga'],
    ['17:00', 'Pilates Reformer'],
    ['18:00', 'Full Body'],
    ['18:00', 'Pilates Reformer'],
    ['18:15', 'Entrenamiento Funcional'],
    ['18:30', 'Ciclismo'],
    ['18:30', 'Hidrotraining'],
    ['19:00', 'Super Local'],
    ['19:00', 'Pilates Reformer'],
    ['19:15', 'Entrenamiento Funcional'],
    ['19:30', 'Ciclismo'],
    ['19:30', 'Hidrotraining'],
    ['20:00', 'Pilates Reformer'],
  ],
  'MJ': [
    ['08:00', 'Pilates Reformer'],
    ['08:00', 'Entrenamiento Funcional'],
    ['09:00', 'Pilates Reformer'],
    ['09:00', 'Ciclismo'],
    ['09:00', 'GAP'],
    ['09:30', 'Hidrotraining'],
    ['10:00', 'Pilates Reformer'],
    ['10:00', 'Postura y Movilidad'],
    ['10:30', 'Hidrotraining'],
    ['11:00', 'Postura y Movilidad'],
    ['14:00', 'Pilates Reformer'],
    ['15:00', 'Pilates Reformer'],
    ['16:00', 'Pilates Reformer'],
    ['17:00', 'Pilates Reformer'],
    ['17:00', 'Postura y Movilidad'],
    ['17:30', 'Hidrotraining'],
    ['18:00', 'Pilates Reformer'],
    ['18:00', 'Fit Pilates'],
    ['18:15', 'Entrenamiento Funcional'],
    ['18:30', 'Ciclismo'],
    ['18:30', 'Hidrotraining'],
    ['19:00', 'Pilates Reformer'],
    ['19:00', 'Full Body'],
    ['19:15', 'Cross Training'],
    ['19:30', 'Ciclismo'],
    ['20:00', 'Pilates Reformer'],
    ['20:00', 'Ritmos'],
  ],
  'Sabado': [
    ['09:00', 'Pilates Reformer'],
    ['09:30', 'Super Local'],
    ['10:00', 'Pilates Reformer'],
    ['10:30', 'Hidrotraining'],
    ['11:00', 'Pilates Reformer'],
    ['11:30', 'Entrenamiento Funcional'],
    ['12:00', 'Pilates Reformer'],
  ],
};

// ─── Horarios de sala ────────────────────────────────────────────────────────
const SALA_HORARIOS = {
  general: [
    { dias: 'Lun – Vie', horario: '06:30 – 22:00' },
    { dias: 'Sábado',    horario: '09:00 – 18:00' },
  ],
  excepciones: {
    sedes:   ['Punta Carretas', 'Punta del Este'],
    entrada: { dias: 'Domingo', horario: '09:00 – 13:00' },
  },
};

// ─── Individual days mapping ─────────────────────────────────────────────────
const DAY_TO_GROUP = {
  'Lunes':     'LMV',
  'Martes':    'MJ',
  'Miércoles': 'LMV',
  'Jueves':    'MJ',
  'Viernes':   'LMV',
  'Sábado':    'Sabado',
};
const INDIVIDUAL_DAYS = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];

// ─── Flatten to CLASES ────────────────────────────────────────────────────────
let _id = 0;
const CLASES = [];
for (const sede of SEDE_NAMES) {
  for (const dia of Object.keys(RAW[sede])) {
    for (const [hora, actividad] of RAW[sede][dia]) {
      CLASES.push({ id: _id++, sede, dia, hora, actividad });
    }
  }
}

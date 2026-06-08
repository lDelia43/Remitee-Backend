# SweetMedical — API Reference

Base URL: `http://localhost:8080`

All responses use `application/json`. Dates are in **UTC ISO 8601** format (`2026-06-10T09:00:00Z`).

---

## Doctors

### Get all doctors

```
GET /doctors
```

**Response `200`**
```json
{
  "doctors": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "John Smith",
      "specialty": "Cardiology"
    }
  ]
}
```

---

## Appointments

### Get appointments by doctor

```
GET /appointments?doctorId={doctorId}&page=1&pageSize=10
```

| Query param | Type   | Required | Default |
|-------------|--------|----------|---------|
| `doctorId`  | `uuid` | ✅       |         |
| `page`      | `int`  | ❌       | `1`     |
| `pageSize`  | `int`  | ❌       | `10`    |

**Response `200`**
```json
{
  "appointments": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "patientName": "Alice Johnson",
      "scheduledAt": "2026-06-10T09:00:00Z",
      "status": "Active"
    }
  ],
  "totalCount": 5,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

---

### Create appointment

```
POST /appointments
Content-Type: application/json
```

**Body**
```json
{
  "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "patientName": "Alice Johnson",
  "scheduledAt": "2026-06-10T09:00:00Z"
}
```

| Field         | Type       | Required | Notes                        |
|---------------|------------|----------|------------------------------|
| `doctorId`    | `uuid`     | ✅       |                              |
| `patientName` | `string`   | ✅       |                              |
| `scheduledAt` | `datetime` | ✅       | Must be a future date (UTC)  |

**Response `201`**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "patientName": "Alice Johnson",
  "scheduledAt": "2026-06-10T09:00:00Z",
  "status": "Active"
}
```

---

### Cancel appointment

```
PATCH /appointments/{id}/cancel
```

| Path param | Type   | Required |
|------------|--------|----------|
| `id`       | `uuid` | ✅       |

**Response `200`**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Cancelled"
}
```

---

## Errors

Todos los errores siguen el formato [RFC 9110 Problem Details](https://www.rfc-editor.org/rfc/rfc9110):

```json
{
  "title": "Cannot create an appointment in the past.",
  "status": 400
}
```

| Status | Causa |
|--------|-------|
| `400`  | Validación fallida (ej: fecha en el pasado, campos requeridos) |
| `404`  | Recurso no encontrado |
| `409`  | El doctor ya tiene un turno activo en ese horario |
| `500`  | Error interno del servidor |

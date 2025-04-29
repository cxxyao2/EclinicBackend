-- Practitioners
INSERT INTO "Practitioners" (practitioner_id, first_name, last_name, specialization, license_number, email, phone_number, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 'John', 'Smith', 'Cardiology', 'LIC001', 'john.smith@example.com', '1234567890', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 'Sarah', 'Johnson', 'Pediatrics', 'LIC002', 'sarah.johnson@example.com', '2345678901', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 'Michael', 'Brown', 'Orthopedics', 'LIC003', 'michael.brown@example.com', '3456789012', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Patients
INSERT INTO "Patients" (patient_id, first_name, last_name, date_of_birth, gender, address, phone_number, email, emergency_contact, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 'Alice', 'Wilson', '1990-05-15', 'Female', '123 Main St', '4567890123', 'alice.wilson@example.com', 'Bob Wilson: 5678901234', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 'David', 'Lee', '1985-08-20', 'Male', '456 Oak Ave', '5678901234', 'david.lee@example.com', 'Mary Lee: 6789012345', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 'Emma', 'Davis', '1995-03-10', 'Female', '789 Pine Rd', '6789012345', 'emma.davis@example.com', 'John Davis: 7890123456', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Medications
INSERT INTO "Medications" (medication_id, name, dosage, route, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 'Amoxicillin', '500mg', 'Oral', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 'Ibuprofen', '200mg', 'Oral', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 'Omeprazole', '20mg', 'Oral', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- PractitionerAvailabilities
INSERT INTO "PractitionerAvailabilities" (available_id, practitioner_id, start_time, end_time, day_of_week, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, '09:00', '17:00', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, '08:00', '16:00', 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, '10:00', '18:00', 3, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Appointments
INSERT INTO "Appointments" (appointment_id, patient_id, available_id, reason_for_visit, status, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 'Regular checkup', 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 'Follow-up visit', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 'Annual physical', 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- PractitionerSchedules
INSERT INTO "PractitionerSchedules" (schedule_id, practitioner_id, patient_id, start_date_time, end_date_time, reason_for_visit, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, CURRENT_TIMESTAMP + INTERVAL '1 day', CURRENT_TIMESTAMP + INTERVAL '1 day' + INTERVAL '1 hour', 'Consultation', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, CURRENT_TIMESTAMP + INTERVAL '2 days', CURRENT_TIMESTAMP + INTERVAL '2 days' + INTERVAL '1 hour', 'Follow-up', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, CURRENT_TIMESTAMP + INTERVAL '3 days', CURRENT_TIMESTAMP + INTERVAL '3 days' + INTERVAL '1 hour', 'Check-up', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- VisitRecords
INSERT INTO "VisitRecords" (visit_id, patient_id, practitioner_id, schedule_id, practitioner_signature_path, visit_date, diagnosis, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 1, '/signatures/sig1.png', CURRENT_TIMESTAMP, 'Hypertension', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 2, '/signatures/sig2.png', CURRENT_TIMESTAMP, 'Common cold', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 3, '/signatures/sig3.png', CURRENT_TIMESTAMP, 'Annual check-up - normal', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Prescriptions
INSERT INTO "Prescriptions" (prescription_id, visit_id, patient_id, practitioner_id, medication_id, dosage, start_date, end_date, instructions, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 1, 1, '500mg 3x daily', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '7 days', 'Take with food', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 2, 2, '200mg as needed', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '5 days', 'Take for pain', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 3, 3, '20mg daily', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '30 days', 'Take before breakfast', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- LabTests
INSERT INTO "LabTests" (lab_test_id, patient_id, practitioner_id, test_name, test_result, test_date, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 'Blood Test', 'Normal', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 'Urinalysis', 'Normal', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 'X-Ray', 'No abnormalities detected', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Inpatients
INSERT INTO "Inpatients" (inpatient_id, patient_id, practitioner_id, nurse_id, admission_date, discharge_date, room_number, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '5 days', 'A101', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP + INTERVAL '3 days', 'B202', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 3, CURRENT_TIMESTAMP, NULL, 'C303', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Beds
INSERT INTO "Beds" (bed_id, room_number, bed_number, inpatient_id, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 'A101', 'Bed1', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 'B202', 'Bed1', 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 'C303', 'Bed1', 3, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Billings
INSERT INTO "Billings" (billing_id, patient_id, billing_date, amount, payment_status, payment_method, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, CURRENT_TIMESTAMP, 500.00, 'Paid', 'Credit Card', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, CURRENT_TIMESTAMP, 300.00, 'Pending', 'Insurance', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, CURRENT_TIMESTAMP, 1000.00, 'Paid', 'Cash', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- ImageRecords
INSERT INTO "ImageRecords" (image_id, patient_id, practitioner_id, inpatient_id, image_type, image_description, image_path, created_at, updated_at, created_by, created_date, last_modified_by, last_modified_date)
VALUES 
(1, 1, 1, 1, 'X-Ray', 'Chest X-Ray', '/images/xray1.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(2, 2, 2, 2, 'MRI', 'Brain MRI', '/images/mri2.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP),
(3, 3, 3, 3, 'CT Scan', 'Abdominal CT', '/images/ct3.jpg', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP, 1, CURRENT_TIMESTAMP);

-- Reset the sequences
SELECT setval('"Practitioners_practitioner_id_seq"', (SELECT MAX(practitioner_id) FROM "Practitioners"));
SELECT setval('"Patients_patient_id_seq"', (SELECT MAX(patient_id) FROM "Patients"));
SELECT setval('"Medications_medication_id_seq"', (SELECT MAX(medication_id) FROM "Medications"));
SELECT setval('"PractitionerAvailabilities_available_id_seq"', (SELECT MAX(available_id) FROM "PractitionerAvailabilities"));
SELECT setval('"Appointments_appointment_id_seq"', (SELECT MAX(appointment_id) FROM "Appointments"));
SELECT setval('"PractitionerSchedules_schedule_id_seq"', (SELECT MAX(schedule_id) FROM "PractitionerSchedules"));
SELECT setval('"VisitRecords_visit_id_seq"', (SELECT MAX(visit_id) FROM "VisitRecords"));
SELECT setval('"Prescriptions_prescription_id_seq"', (SELECT MAX(prescription_id) FROM "Prescriptions"));
SELECT setval('"LabTests_lab_test_id_seq"', (SELECT MAX(lab_test_id) FROM "LabTests"));
SELECT setval('"Inpatients_inpatient_id_seq"', (SELECT MAX(inpatient_id) FROM "Inpatients"));
SELECT setval('"Beds_bed_id_seq"', (SELECT MAX(bed_id) FROM "Beds"));
SELECT setval('"Billings_billing_id_seq"', (SELECT MAX(billing_id) FROM "Billings"));
SELECT setval('"ImageRecords_image_id_seq"', (SELECT MAX(image_id) FROM "ImageRecords"));
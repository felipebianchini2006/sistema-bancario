CREATE DATABASE bankdb
  WITH ENCODING='UTF8' LC_COLLATE='C' LC_CTYPE='C' TEMPLATE=template0;

\connect bankdb;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS users (
  id SERIAL PRIMARY KEY,
  email TEXT UNIQUE NOT NULL,
  password_hash TEXT NOT NULL,
  role TEXT NOT NULL DEFAULT 'customer',
  created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS accounts (
  id SERIAL PRIMARY KEY,
  user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  number TEXT UNIQUE NOT NULL,
  balance NUMERIC(18,2) NOT NULL DEFAULT 0,
  created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

DO $$ BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'tx_type') THEN
      CREATE TYPE tx_type AS ENUM ('credit','debit');
   END IF;
END $$;

CREATE TABLE IF NOT EXISTS transactions (
  id SERIAL PRIMARY KEY,
  account_id INT NOT NULL REFERENCES accounts(id) ON DELETE CASCADE,
  type tx_type NOT NULL,
  amount NUMERIC(18,2) NOT NULL CHECK (amount > 0),
  description TEXT,
  created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS audit_logs (
  id SERIAL PRIMARY KEY,
  user_id INT,
  action TEXT NOT NULL,
  details TEXT,
  ts TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

INSERT INTO users (email, password_hash, role)
VALUES ('admin@bank.local', '$2a$11$KcMYJp3oYq7i9G8m8lthne9r8m5A1yJ5v4w8m2x3pQfL8Kf6g5b5W', 'admin')  -- senha: Admin@123
ON CONFLICT DO NOTHING;

INSERT INTO accounts (user_id, number, balance)
SELECT id, '0001-0000001', 1000.00 FROM users WHERE email='admin@bank.local'
ON CONFLICT DO NOTHING;

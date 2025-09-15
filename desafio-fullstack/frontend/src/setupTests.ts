import { beforeAll, afterEach, afterAll } from 'vitest';
import 'whatwg-fetch';
import '@testing-library/jest-dom';
import { server } from './tests/msw/server';

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());

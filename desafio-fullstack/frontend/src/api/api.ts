import type { Lead, LeadStatus } from '../types/types';

const API = 'http://localhost:5080/api';

export async function fetchLeads(status: LeadStatus): Promise<Lead[]> {
  const res = await fetch(`${API}/leads?status=${status}`);
  if (!res.ok) throw new Error('Erro ao buscar leads');
  return res.json();
}

export async function acceptLead(id: number): Promise<void> {
  const res = await fetch(`${API}/leads/${id}/accept`, { method: 'POST' });
  if (!res.ok && res.status !== 204) throw new Error('Erro ao aceitar lead');
}

export async function declineLead(id: number): Promise<void> {
  const res = await fetch(`${API}/leads/${id}/decline`, { method: 'POST' });
  if (!res.ok && res.status !== 204) throw new Error('Erro ao recusar lead');
}

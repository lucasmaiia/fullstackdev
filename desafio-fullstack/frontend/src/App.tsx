import { useEffect, useState } from 'react';
import { fetchLeads, acceptLead, declineLead } from './api/api';
import type { Lead, LeadStatus } from './types/types';
import { LeadCard } from './components/LeadCard';
import './styles/main.css';

interface TabButtonProps {
  active: boolean;
  onClick: () => void;
  children: React.ReactNode;
}

function TabButton({ active, onClick, children }: TabButtonProps) {
  return (
    <button className={`tab ${active ? 'active' : ''}`} onClick={onClick}>
      {children}
    </button>
  );
}

export default function App() {
  const [tab, setTab] = useState<LeadStatus>('New');
  const [data, setData] = useState<Lead[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [err, setErr] = useState<string | null>(null);

  async function load(status: LeadStatus) {
    setLoading(true); setErr(null);
    try { setData(await fetchLeads(status)); }
    catch (e: unknown) { setErr(e instanceof Error ? e.message : 'Erro'); }
    finally { setLoading(false); }
  }

  useEffect(() => { void load(tab); }, [tab]);

  async function onAccept(id: number) { await acceptLead(id); await load('New'); }
  async function onDecline(id: number) { await declineLead(id); await load('New'); }

  return (
    <div className="container">
      <h1 className='title'>Lead Manager Full Stack</h1>

      <div className="tabs">
        <TabButton active={tab === 'New'} onClick={() => setTab('New')}>Invited</TabButton>
        <TabButton active={tab === 'Accepted'} onClick={() => setTab('Accepted')}>Accepted</TabButton>
      </div>

      {loading && <p>Carregando...</p>}
      {err && <p style={{ color: 'red' }}>{err}</p>}
      {!loading && !err && data.length === 0 && <p>Sem leads.</p>}

      {data.map((lead) => (
        <LeadCard key={lead.id} lead={lead} onAccept={onAccept} onDecline={onDecline} tab={tab} />
      ))}

    </div>
  );
}

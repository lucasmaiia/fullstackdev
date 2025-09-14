import type { Lead, LeadStatus } from '../types/types';

function IconLocation() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M12 22s7-6.14 7-11.5A7 7 0 1 0 5 10.5C5 15.86 12 22 12 22z" stroke="currentColor" strokeWidth="1.6"/>
      <circle cx="12" cy="10.5" r="2.5" stroke="currentColor" strokeWidth="1.6"/>
    </svg>
  );
}
function IconBriefcase() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M3 9h18v10a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V9z" stroke="currentColor" strokeWidth="1.6"/>
      <path d="M8 9V6a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v3" stroke="currentColor" strokeWidth="1.6"/>
    </svg>
  );
}
function IconTag() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M20 12l-8 8-8-8 8-8 8 8z" stroke="currentColor" strokeWidth="1.6"/>
    </svg>
  );
}
function IconPhone() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M22 16.92v2a2 2 0 0 1-2.18 2 19.8 19.8 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6A19.8 19.8 0 0 1 2.09 3.18 2 2 0 0 1 4.1 1h2a2 2 0 0 1 2 1.72c.12.9.33 1.77.62 2.6a2 2 0 0 1-.45 2.11L7.1 8.9a16 16 0 0 0 6 6l1.47-1.17a2 2 0 0 1 2.11-.45c.83.29 1.7.5 2.6.62A2 2 0 0 1 22 16.92z" stroke="currentColor" strokeWidth="1.6"/>
    </svg>
  );
}
function IconMail() {
  return (
    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
      <path d="M4 4h16a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2z" stroke="currentColor" strokeWidth="1.6"/>
      <path d="M22 6l-10 7L2 6" stroke="currentColor" strokeWidth="1.6"/>
    </svg>
  );
}

export function LeadCard({
  lead,
  onAccept,
  onDecline,
  tab
}: {
  lead: Lead;
  onAccept?: (id: number) => void;
  onDecline?: (id: number) => void;
  tab?: LeadStatus;
}) {
  const fullName = `${lead.firstName} ${lead.lastName}`.trim();
  const initial = (lead.firstName || '?').slice(0, 1).toUpperCase();

  return (
    <div className="card">
      {/* Header */}
      <div className="card-header">
        <div className="avatar">{initial}</div>
        <div>
          <div className="name">
            {tab === 'Accepted' ? (fullName || lead.firstName) : lead.firstName}
          </div>
          <div className="small-muted">
            {new Date(lead.dateCreated).toLocaleString('pt-BR')}
          </div>
        </div>
      </div>

      <div className="row">
        <span className="chip"><IconLocation /> {lead.suburb}</span>
        <span className="chip"><IconBriefcase /> {lead.category}</span>
        <span className="chip"><IconTag /> Job ID: {lead.id}</span>
      </div>

      {/* Faixa de contato – apenas quando Accepted */}
      {lead.status === 'Accepted' && (
        <div className="contact-row">
          {lead.phone && (
            <a className="contact-link" href={`tel:${(lead.phone || '').replace(/\D/g, '')}`}>
              <IconPhone /> {lead.phone}
            </a>
          )}
          {lead.email && (
            <a className="contact-link" href={`mailto:${lead.email}`}>
              <IconMail /> {lead.email}
            </a>
          )}
        </div>
      )}

      <div className="desc">{lead.description}</div>

      <div className="footer">
        {lead.status === 'New' ? (
          <div style={{ display: 'flex', gap: 8 }}>
            <button className="btn btn-primary" onClick={() => onAccept?.(lead.id)}>Accept</button>
            <button className="btn btn-secondary" onClick={() => onDecline?.(lead.id)}>Decline</button>
          </div>
        ) : <div />}

        <div className="price">
          ${lead.price.toFixed(2)} <span className="small-muted">Lead Invitation</span>
        </div>
      </div>
    </div>
  );
}

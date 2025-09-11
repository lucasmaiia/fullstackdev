import type { Lead, LeadStatus } from './types';

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
          <div className="name">{tab === 'Accepted' ? (fullName || lead.firstName) : lead.firstName}</div>
          <div className="small-muted">
            {new Date(lead.dateCreated).toLocaleString()}
          </div>
        </div>
      </div>

      {/* Row icons */}
      <div className="row">
        <span className="chip"><IconLocation /> {lead.suburb}</span>
        <span className="chip"><IconBriefcase /> {lead.category}</span>
        <span className="chip"><IconTag /> Job ID: {lead.id}</span>
      </div>

      {/* Descrição */}
      <div className="desc">{lead.description}</div>

      {/* Footer: buttons + price */}
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

      {/* Campos extras quando aceito */}
      {lead.status === 'Accepted' && (
        <div style={{ marginTop: 10 }}>
          <div className="small-muted"><strong>Phone:</strong> {lead.phone || '—'}</div>
          <div className="small-muted"><strong>Email:</strong> {lead.email || '—'}</div>
        </div>
      )}
    </div>
  );
}

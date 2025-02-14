import { useState, useEffect } from 'react';
import reactLogo from './assets/react.svg';
import viteLogo from '/vite.svg';
import './App.css';
import Expenses from './components/Expenses';
import ExpenseForm from './components/ExpenseForm';
import { getExpenses } from './services/api';

function App() {
  const [showForm, setShowForm] = useState(false);
  const [expenses, setExpenses] = useState<any[]>([]);

  useEffect(() => {
    fetchExpenses();
  }, []);

  const fetchExpenses = async () => {
    const data = await getExpenses();
    setExpenses(data);
  };

  const handleSave = (newExpense: any) => {
    setExpenses([...expenses, newExpense]);
    setShowForm(false);
  };

  const handleDelete = (id: string) => {
    setExpenses(expenses.filter(expense => expense.id !== id));
  };

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setShowForm(!showForm)}>
          {showForm ? 'Cancel' : 'Add Expense'}
        </button>
        {showForm && <ExpenseForm onSave={handleSave} />}
        <Expenses expenses={expenses} onDelete={handleDelete}/>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  );
}

export default App;
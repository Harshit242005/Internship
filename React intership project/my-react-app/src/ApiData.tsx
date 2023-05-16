import React, { useState, useEffect } from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Link } from 'react-router-dom'

interface Post {
  userId: number;
  id: number;
  title: string;
  body: string;
}

const SecondPage = () => {
  const [posts, setPosts] = useState<Post[]>([]);

  const columns: GridColDef[] = [

    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'title', headerName: 'Title', width: 300 },
    { field: 'body', headerName: 'Body', width: 600 },
  ];

  useEffect(() => {
    fetch('https://jsonplaceholder.typicode.com/posts')
      .then(response => response.json())
      .then(data => {
        console.log('Data has been received:', data);
        setPosts(data);
        console.log('Posts state:', [...posts, ...data]);
      })
      .catch(error => console.error(error));
  }, []);

  return (
    <div style={{ height: 700, width: '100%' }}>
      <Link to="/"><button style={{ backgroundColor: 'black', color: 'white', padding: '10px 20px', width: '200px', height: '50px' }}>Go back</button></Link>
      <DataGrid rows={posts} columns={columns} />
    </div>
  );
};

export default SecondPage;

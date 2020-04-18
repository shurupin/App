import React from 'react';
import { Datagrid, EmailField, List, TextField, NumberField, TextInput, EditButton, Filter } from 'react-admin';

const UserFilter = (props: any) => (
    <Filter {...props}>
        <TextInput label="Name" source="Name" />
        <TextInput label="Email" source="Email" />
    </Filter>
);

const UserList = (props: any) => (
    <List {...props} filters={<UserFilter />} sort={{ field: 'id', order: 'ASC' }}>
        <Datagrid>
            <NumberField source="id" />
            <TextField source="name" />
            <EmailField source="email" />
            <EditButton />
        </Datagrid>
    </List>
);

export default UserList;
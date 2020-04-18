import React from 'react';
import { SimpleForm, TextInput, Create } from 'react-admin';

const UserCreate = (props: any) => (
    <Create {...props}>
        <SimpleForm>
            <TextInput source="name" />
            <TextInput source="email" />
        </SimpleForm>
    </Create>
);

export default UserCreate;
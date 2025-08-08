# WebWorkerService

## ��������

������ ������������ �� ���� ���������� ������� ���-�����

## �����������

UI - Blazor App  
Job - Quartz

## �������������

### ������������

����������� ������� ��������, ���������� ������ ������.
����� �������� ��������� ������ ������. ��� ��������� ������� �����
���������������� ���������� (���������� �� ������, ��� ����������� ����������)

��������� ������� ������� ����������� ���������� (Enabled, VisibleOneStart, Cron)

### ����������

������������ ������ � DI
```csharp
builder.AddWorkers();
```
��������� ������ ����� ������� ����������
```csharp
await app.RunWorkersAsync();
```

### ���������� �����

������ ������ ������ ������������� �� ������������ ������.
��������� ����������� ����� RunAsync.  

����� ����� ���������� � DI.

```csharp
    public class TestJob : BaseConcurrentCancelJob
    {
        private readonly TestService _service;

        public TestJob(TestService service)
        {
            _service = service;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            // ������
        }
    }
```

�������� ����������� �������:  

- __BaseJob__  
������� ���������� ������.

- __BaseSingleJob__  
���������� ����� ������. ��������� ������� ����������.

- __BaseConcurrentJob__  
���������� ��������������� ������. ��������� ������� ����������.

- __BaseSingleCancelJob__  
���������� ���������� ����� ������.

- __BaseConcurrentCancelJob__  
���������� ���������� ��������������� ������.
